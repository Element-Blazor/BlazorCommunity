using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using System.IO;

namespace BlazorCommunity.Api
{
    public class Program
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddCommandLine(args)//÷ß≥÷√¸¡Ó––
          .Build();

            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((context, loggingBuilder) => loggingBuilder.AddNLog())
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
             .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}