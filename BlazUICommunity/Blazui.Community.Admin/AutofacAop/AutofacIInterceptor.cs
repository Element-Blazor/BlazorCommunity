//using Blazui.Community.Admin.Service;
//using Castle.DynamicProxy;
//using Microsoft.AspNetCore.Components;
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Text;

//namespace Blazui.Community.Admin.AutofacAop
//{
//    public class AutofacIInterceptor : IInterceptor
//    {
//        private readonly AdminUserService userService;
//        public AutofacIInterceptor(AdminUserService UserService)
//        {
//            userService = UserService;
//        }
//        public void Intercept(IInvocation invocation)
//        {
//            if(userService is null)
//            {

//            }
//            //var SuperAdministrator = invocation.Method.GetCustomAttribute<SuperAdminAuthorizeAttribute>(true);
//            //if (SuperAdministrator != null || !userService.IsSupperAdmin1())
//            //{
//            //    return;
//            //}

//            Console.WriteLine($"被标记方法执行前执行  invocation.Arguments:");

//            return;
//            invocation.Proceed();// 可以理解成一个占位符   调用原来的方法去了  

//            Console.WriteLine($"被标记方法执行后执行 invocation.Arguments");
//        }
//    }
//}
