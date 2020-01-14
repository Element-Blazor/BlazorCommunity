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

namespace BlazUICommunity.Api
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
          .AddCommandLine(args)//支持命令行
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
            .ConfigureLogging((context , loggingBuilder) =>
            {
                loggingBuilder.AddFilter("System" , LogLevel.Warning);
                loggingBuilder.AddFilter("Microsoft" , LogLevel.Warning);//过滤掉系统默认的一些日志
                //loggingBuilder.AddLog4Net();// log4 加载配置文件
                //loggingBuilder.AddConfiguration()
                //loggingBuilder.AddNLog();
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
             .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
