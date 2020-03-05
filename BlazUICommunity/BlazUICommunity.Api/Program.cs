using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Blazui.Community.Api
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
