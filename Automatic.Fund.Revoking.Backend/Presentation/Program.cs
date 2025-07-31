using Infrastructure.Persistence.Providers.EntityFramework.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Presentation.Extensions;
using Serilog;
using System;
using WebCore.Loggers.Serilog.Configuration;
using Microsoft.Extensions.Logging;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //environment = "development";
            var config = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json", true, true)
                     .AddJsonFile($"appsettings.{environment}.json", true, true)
                     .Build();

            Log.Logger = SerilogConfiguration.CreateLogger(config);
            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args, config).Build().Migrate<FundContext>().Run();

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex,"Error occured");
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot config) =>
                Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    // Add services to the container.
                    //webBuilder.UseUrls("https://*:5001", "http://*:5000");

                    var url = config.GetSection("App:Url").Value;

                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(url);
                    webBuilder.UseKestrel(option => option.AddServerHeader = false);

                })
                .ConfigureLogging(logging => logging.AddConsole())
                ;

    }
}
