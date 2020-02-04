﻿using Autofac;
using System;
using System.Linq;
using System.Reflection;

namespace Blazui.Community.Utility
{
    public class CustomAutofacModule : Autofac.Module
    {
    
        protected override void Load(ContainerBuilder containerBuilder)
        {

       

            //程序集注入
            Assembly serviceAss = Assembly.Load("Blazui.Community.Repository");
            Type[] sertypes = serviceAss.GetTypes().Where(p => p.Name.EndsWith("Repository")).ToArray();
            containerBuilder.RegisterTypes(sertypes).AsImplementedInterfaces().PropertiesAutowired();
            containerBuilder.RegisterAssemblyTypes(serviceAss);
            Assembly interfaceAss = Assembly.Load("Blazui.Community.UnitOfWork");
            //containerBuilder.RegisterAssemblyTypes(interfaceAss);
            Type[] interfacetypes = interfaceAss.GetTypes().Where(p => p.Name.EndsWith("Repository")).ToArray();
            containerBuilder.RegisterTypes(interfacetypes).AsImplementedInterfaces().PropertiesAutowired();


            //单个注入
            //containerBuilder.RegisterType<UserService>().As<IUserService>().EnableInterfaceInterceptors();

            containerBuilder.Register(c => new CustomAutofacAop());//aop注册


        }

    }
}
