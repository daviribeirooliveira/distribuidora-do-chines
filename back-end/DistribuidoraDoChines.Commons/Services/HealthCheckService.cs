using System;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace DistribuidoraDoChines.Commons.Services
{
    public static class HealthCheckService
    {
        public static void UseHealthCheckService(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/status",
                new HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonConvert.SerializeObject(
                            new
                            {
                                statusApplication = report.Status.ToString(),
                                healthChecks = report.Entries.Select(e => new
                                {
                                    check = e.Key,
                                    ErrorMessage = e.Value.Exception?.Message,
                                    status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                                })
                            });

                        context.Response.ContentType = MediaTypeNames.Application.Json;

                        await context.Response.WriteAsync(result);
                    }
                });
        }
    }
}