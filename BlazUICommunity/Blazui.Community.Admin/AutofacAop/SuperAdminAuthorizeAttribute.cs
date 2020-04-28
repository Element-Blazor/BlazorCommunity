//using AspectCore.DynamicProxy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AspectCore.Extensions.DependencyInjection;
//using AspectCore.DependencyInjection;
//using Blazui.Community.Admin.Service;
//using System.Reflection;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Authorization;

//namespace Blazui.Community.Admin.AutofacAop
//{
//    public class SuperAdminAuthorizeAttribute : AbstractInterceptorAttribute
//    {
//        [FromServiceContext]
//        public AdminUserService userService { get; set; }

//        [FromServiceContext]
//        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
//        public override async Task Invoke(AspectContext context, AspectDelegate next)
//        {
//            try
//            {
//                //var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
//                if (! await userService.IsSupperAdmin())
//                {

//                }
//                Console.WriteLine("Before service call");
//                await next(context);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Service threw an exception!");
//                throw;
//            }
//            finally
//            {
//                Console.WriteLine("After service call");
//            }
//        }
//    }
//}
