using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OneTree.Assessment.Domain;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace OneTree.Assessment.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .UseSerilog((hostBuilderContext, loggerConfig) =>
             {
                 loggerConfig.MinimumLevel.Information()
                     .ReadFrom.Configuration(hostBuilderContext.Configuration)
                     .WriteTo.Console()
                     .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(hostBuilderContext.Configuration.GetValue<string>("ElasticServer")))
                     {
                         AutoRegisterTemplate = true,
                         AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
                     })
                     .WriteTo.File($"OneTree.Assessment.Api-{DateTime.Now.Millisecond}.log", rollingInterval: RollingInterval.Day);
             })
            .UseStartup<Startup>();
    }
}
