using Autofac;
using System;
using System.Linq;
using System.Reflection;

namespace BlazorCommunity.AutofacModules
{
    public class CustomAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            //程序集注入
            Assembly serviceAss = Assembly.Load("BlazorCommunity.Repository");
            Type[] sertypes = serviceAss.GetTypes().Where(p => p.Name.EndsWith("Repository")).ToArray();
            containerBuilder.RegisterTypes(sertypes).AsImplementedInterfaces().PropertiesAutowired();
            containerBuilder.RegisterAssemblyTypes(serviceAss);
            Assembly interfaceAss = Assembly.Load("BlazorCommunity.UnitOfWork");
            //containerBuilder.RegisterAssemblyTypes(interfaceAss);
            Type[] interfacetypes = interfaceAss.GetTypes().Where(p => p.Name.EndsWith("Repository")).ToArray();
            containerBuilder.RegisterTypes(interfacetypes).AsImplementedInterfaces().PropertiesAutowired();
        }
    }
}