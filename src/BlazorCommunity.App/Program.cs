using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace BlazorCommunity.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureLogging((context, loggingBuilder) =>
             {
                 loggingBuilder.AddFilter("System", LogLevel.Warning);
                 loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);
                 loggingBuilder.AddConsole();
                 //loggingBuilder.AddDebug();
                 //loggingBuilder.AddEventLog();
                 loggingBuilder.AddNLog();
             })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseNLog();

        //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    }
}