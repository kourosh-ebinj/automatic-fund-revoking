using Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Configuration;
using Presentation.Controllers;
using WebCore.Extensions;
using System;
using System.Text.Json;
using WebCore.Configuration;
using WebCore.Filters;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Infrastructure.Persistence.Providers.EntityFramework.Contexts;
using Serilog;
using WebCore.Helpers;
using Core;
using Presentation.Configuration.DI;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Presentation.Extensions;
using Presentation.Jobs;
using Application.Models;
using System.Text.Json.Serialization;
using Core.Abstractions.Caching;
using System.Collections.Generic;
using Hangfire.Dashboard;
using Presentation.Scheduler;
using System.Reflection;

namespace Presentation
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;
        private ApplicationSettingExtenderModel _applicationExtenderSetting;
        private IWebHostEnvironment _environment;

        public Startup(IWebHostEnvironment env)
        {
            _environment = env;

            var builder = new ConfigurationBuilder();
            builder.Config(env,
                           out _configuration,
                           out _applicationExtenderSetting);
            _applicationExtenderSetting = new ApplicationSettingExtenderModel();
            _configuration.Bind(_applicationExtenderSetting);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalErrorHandler>();
            services.AddProblemDetails();
            services.AddSerilog();

            //services.AddDbContextPool<FundContext>(options =>
            //{
            //    options.UseSqlServer(_configuration.GetConnectionString("SqlConnection"),
            //    sqlServerOptionsAction: sqlOptions =>
            //    {
            //        sqlOptions.EnableRetryOnFailure(
            //        maxRetryCount: 3,
            //        maxRetryDelay: TimeSpan.FromMilliseconds(10),
            //        errorNumbersToAdd: null);
            //    });
            //});

            services.AddDbContext<FundContext>((sp, options) =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("SqlConnection"),
                //options.EnableSensitiveDataLogging(),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromMilliseconds(10),
                    errorNumbersToAdd: null);
                })
                ;
            }, ServiceLifetime.Scoped);

            // register our own implementation of the ContextFactory
            // it pulls the default PooledFactory and returns a new DbContext
            // with TenantID set
            //services.AddScoped<SampleContextFactory>();
            //services.AddScoped(sp => sp.GetRequiredService<SampleContextFactory>().CreateDbContext());

            services.AddOptions();
            services.AddHttpContextAccessor();
            services.AddHealthChecks();
            //.AddCheck<>healthcheck;

            var autoMapperConfig = new MapperConfiguration(cfg =>
                cfg.AddMaps(
                    nameof(Domain),
                    nameof(Application),
                    nameof(Infrastructure),
                    nameof(WebCore),
                    nameof(Presentation)
                ));
            var mapper = autoMapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            #region ApplicationSettingModel Binding
            services.AddSingleton(typeof(ApplicationSettingExtenderModel), _applicationExtenderSetting);
            services.AddSingleton(typeof(ApplicationSettingModel), _applicationExtenderSetting);
            #endregion

            services.ConfigResponseCompression();

            services.AddServices(_applicationExtenderSetting);

            services.AddHttpClients(_applicationExtenderSetting);

            services.ConfigHangfire(_configuration, _applicationExtenderSetting.App.Name, _environment);

            services.AddControllers(options =>
            {
                options.Filters.Add(new AssignCorrelationId());
            })
                //.AddControllersAsServices()
                .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                //options.JsonSerializerOptions.MaxDepth = 2;

            });
            services.AddMvc() //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .AddFluentValidation(fv =>
                    {
                        //Request Validators
                        fv.ImplicitlyValidateChildProperties = true;
                        fv.RegisterValidatorsFromAssembly(typeof(Startup).Assembly);

                    });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(o => o.AddPolicy(Constants.CorsPolicyName, builder =>
            {
                builder.WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddSwaggerConfig<FundsController>(typeof(Startup), _applicationExtenderSetting);

            services.AddMassTransit(_applicationExtenderSetting, 
                Assembly.Load("Infrastructure"), Assembly.Load("Presentation"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            ServiceLocator.Configure(app.ApplicationServices, _applicationExtenderSetting);

            //var scope = app.ApplicationServices.CreateScope();

            app.UseExceptionHandler();

            app.UseSwaggerConfig<Startup>(_applicationExtenderSetting);

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();

                // Swagger API Documentation
                //app.UseOpenApi();
                //app.UseSwaggerUi3();
                app.UseSwaggerConfig<Startup>(_applicationExtenderSetting);
            }
            else
            {
                app.UseHsts();
            }

            app.UseStatusCodePages(async context => await context.UseHttpStatusCodePagesAsync(app, loggerFactory, env));

            app.UseHttpsRedirection();

            app.UseRouting();

            // Write streamlined request completion events, instead of the more verbose ones from the framework.
            // To use the default framework request logging instead, remove this line and set the "Microsoft"
            // level in appsettings.json to "Information".
            app.UseSerilogRequestLogging();

            app.UseCors(Constants.CorsPolicyName);
            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                app.UseHealthChecks(_applicationExtenderSetting.App.Name, "1.0");

                var hangfireAuthorizationFilters = new List<IDashboardAuthorizationFilter>();
                hangfireAuthorizationFilters.Add(new HangFireDashboardLANAuthorizationFilter());
                endpoints.MapHangfireDashboard(
                    new Hangfire.DashboardOptions()
                    { 
                        IgnoreAntiforgeryToken = true,
                        Authorization = hangfireAuthorizationFilters
                    });
            });

            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new HangFireAuthorizationFilter() },
            //});

            app.UseResponseCompression();


            using var cacheScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            cacheScope.ServiceProvider.GetService<ICacheProvider>().Initialize(_applicationExtenderSetting.App.Name);

            JobScheduler.ScheduleJobs(app, _applicationExtenderSetting);

        }
    }
}
