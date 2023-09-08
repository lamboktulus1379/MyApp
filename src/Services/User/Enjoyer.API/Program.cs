using Enjoyer.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Filters;
using System;

namespace Enjoyer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationUserContext>();
                db.Database.Migrate();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog((context, configuration) =>
            {
                configuration.Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
                //{
                //    AutoRegisterTemplate = true,
                //    IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                //    NumberOfShards = 2,
                //    NumberOfReplicas = 1,
                //    ModifyConnectionSettings = x => x.BasicAuthentication(context.Configuration["ElasticConfiguration:Username"], context.Configuration["ElasticConfiguration:Password"]),
                //    MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information,
                //    TypeName = null,
                //    BatchAction = ElasticOpType.Create
                //})
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .ReadFrom.Configuration(context.Configuration)
                //.Filter.ByExcluding(Matching.WithProperty<string>("RequestPath", v => "/".Equals(v, StringComparison.OrdinalIgnoreCase)))
                ;
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
