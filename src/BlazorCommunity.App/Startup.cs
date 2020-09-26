using Autofac;
using BlazorCommunity.App.Extensions;
using BlazorCommunity.App.Middleware;
using BlazorCommunity.App.Model;
using BlazorCommunity.AppDbContext;
using BlazorCommunity.AutofacModules;
using BlazorCommunity.IdentityExtensions;
using BlazorCommunity.Model.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.LayoutRenderers;
using System;
using System.Text;

namespace BlazorCommunity.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public async void ConfigureServices(IServiceCollection services)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.AddDbContext<BlazorCommunityContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DbConnectionString")));
                services.AddCustomAspIdenitty<BZUserModel, BlazorCommunityContext>();

            services.AddHttpContextAccessor();
            services.AddHttpClient("ElementCommunitiyApp",
              client => client.BaseAddress = new Uri(Configuration["ServerUrl"]));

          
            services.AddMemoryCache();
            services.AddRazorPages();
            services.AddMvc();
            services.AddControllers();
            services.AddServerSideBlazor();

            services.AddElement();
            services.AddCustomService();

            //services.Configure<OpenIdConnectOptions>(option => {
            //});
            services.Configure<TopNavMenuOption>(Configuration);

        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public IContainer AutofacContainer;

        ///// <summary>
        ///// 系统调用
        ///// </summary>
        ///// <param name="builder"></param>
        //public void ConfigureContainer(ContainerBuilder builder)
        //{
        //    builder.RegisterModule<CustomAutofacModule>();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
    
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            LayoutRenderer.Register("basedir", p => env.ContentRootPath);
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<SeoMiddleware>();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}