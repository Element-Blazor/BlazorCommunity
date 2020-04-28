using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AspectCore.Extensions.DependencyInjection;

namespace Blazui.Community.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             //.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}