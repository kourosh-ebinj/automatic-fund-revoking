using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using Core.Constants;
using Newtonsoft.Json;

namespace WebCore.Extensions
{
    public static class IApplicationBuilderExtensions
    {

        /// <summary>Configure the healthcheck http request pipeline</summary>
        /// <param name="app"></param>
        /// <param name="title"></param>
        /// <param name="version"></param>
        public static void UseHealthChecks(this IApplicationBuilder app, string title, string version)
        {
            app.UseHealthChecks((PathString)GlobalConstants.HealthCheckRoute, new HealthCheckOptions()
            {
                ResponseWriter = (Func<HttpContext, HealthReport, Task>)((context, result) =>
                {
                    context.Response.ContentType = "application/json";
                    JObject jobject3 = new JObject(new object[3]
                    {
            new JProperty("status",  result.Status.ToString()),
            new JProperty("totalDuration", result.TotalDuration),
            new JProperty("entries", new JObject(result.Entries.Select<KeyValuePair<string, HealthReportEntry>, JProperty>((Func<KeyValuePair<string, HealthReportEntry>, JProperty>) (pair =>
            {
              JObject content = new JObject(new object[3]
              {
                 new JProperty("status",  pair.Value.Status.ToString()),
                 new JProperty("duration",  pair.Value.Duration),
                 new JProperty("tags",  pair.Value.Tags)
              });
              HealthReportEntry healthReportEntry;
              if (!string.IsNullOrEmpty(pair.Value.Description))
              {
                JObject jobject4 = content;
                healthReportEntry = pair.Value;
                JToken description = (JToken) healthReportEntry.Description;
                jobject4.Add("description", description);
              }
              healthReportEntry = pair.Value;
              if (!string.IsNullOrEmpty(healthReportEntry.Exception?.Message))
              {
                JObject jobject5 = content;
                healthReportEntry = pair.Value;
                JToken message = (JToken) healthReportEntry.Exception?.Message;
                jobject5.Add("exception", message);
              }
              healthReportEntry = pair.Value;
              int num;
              if (healthReportEntry.Data != null)
              {
                healthReportEntry = pair.Value;
                num = healthReportEntry.Data.Count > 0 ? 1 : 0;
              }
              else
                num = 0;
              if (num != 0)
              {
                JObject jobject6 = content;
                healthReportEntry = pair.Value;
                JObject jobject7 = new JObject( healthReportEntry.Data.Select((Func<KeyValuePair<string, object>, JProperty>) (p => new JProperty(p.Key, p.Value))));
                jobject6.Add("data", (JToken) jobject7);
              }
              return new JProperty(pair.Key, content);
            }))))
                    });
                    return context.Response.WriteAsync(jobject3.ToString(Formatting.Indented));
                })
            });

            app.UseHealthChecks((PathString)GlobalConstants.HealthCheckLiveRoute, new HealthCheckOptions()
            {
                Predicate = (r) => r.Tags.Contains("ready"),
                ResponseWriter = (Func<HttpContext, HealthReport, Task>)((context, result) =>
                {
                    context.Response.ContentType = "application/json";
                    JObject jobject = new JObject(new object[2]
                    {
            new JProperty("status", (object)(result.Status == HealthStatus.Healthy ? "live" : "dead")),
            new JProperty("totalDuration", result.TotalDuration)
                    });
                    return context.Response.WriteAsync(jobject.ToString(Formatting.Indented));
                })
            });

            app.UseHealthChecks((PathString)GlobalConstants.HealthCheckReadyRoute, new HealthCheckOptions()
            {
                Predicate = (r) => r.Tags.Contains("version"),
                ResponseWriter = (Func<HttpContext, HealthReport, Task>)((context, result) =>
                {
                    context.Response.ContentType = "application/json";
                    JObject jobject10 = new JObject(new object[2]
                    {
             new JProperty("status", result.Status.ToString()),
             new JProperty("results", new JObject( result.Entries.Select<KeyValuePair<string, HealthReportEntry>, JProperty>((Func<KeyValuePair<string, HealthReportEntry>, JProperty>)
             (pair =>
            {
              JObject content = new JObject(new object[3]
              {
                 new JProperty("status", (object) pair.Value.Status.ToString()),
                 new JProperty("totalDuration", (object) pair.Value.Duration),
                 new JProperty("tags", (object) pair.Value.Tags)
              });
              HealthReportEntry healthReportEntry;
              if (!string.IsNullOrEmpty(pair.Value.Description))
              {
                JObject jobject11 = content;
                healthReportEntry = pair.Value;
                JToken description = (JToken) healthReportEntry.Description;
                jobject11.Add("description", description);
              }
              healthReportEntry = pair.Value;
              if (!string.IsNullOrEmpty(healthReportEntry.Exception?.Message))
              {
                JObject jobject12 = content;
                healthReportEntry = pair.Value;
                JToken message = (JToken) healthReportEntry.Exception?.Message;
                jobject12.Add("exception", message);
              }
              healthReportEntry = pair.Value;
              int num;
              if (healthReportEntry.Data != null)
              {
                healthReportEntry = pair.Value;
                num = healthReportEntry.Data.Count > 0 ? 1 : 0;
              }
              else
                num = 0;
              if (num != 0)
              {
                JObject jobject13 = content;
                healthReportEntry = pair.Value;
                JObject jobject14 = new JObject( healthReportEntry.Data.Select<KeyValuePair<string, object>, JProperty>((Func<KeyValuePair<string, object>, JProperty>) (p => new JProperty(p.Key, p.Value))));
                jobject13.Add("data", (JToken) jobject14);
              }
              return new JProperty(pair.Key, content);
            }))))
                    });
                    return context.Response.WriteAsync(jobject10.ToString(Formatting.Indented));
                })
            });

            app.Map(GlobalConstants.HealthCheckVersionRoute, (Action<IApplicationBuilder>)(appBuilder => appBuilder.Run((RequestDelegate)(async context =>
            {
                JObject json = new JObject(new object[4]
                {
           new JProperty("service", title),
           new JProperty("env", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")),
           new JProperty("machine", Environment.MachineName),
           new JProperty(nameof (version), version)
                });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json.ToString(Formatting.Indented));
                json = (JObject)null;
            }))));

            app.UseHealthChecksPrometheusExporter((PathString)"/metrics");
        }


    }
}
