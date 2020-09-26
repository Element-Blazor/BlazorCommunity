//using AspectCore.DependencyInjection;
//using AspectCore.DynamicProxy;
//using BlazorCommunity.Admin.Service;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BlazorCommunity.Admin.AutofacAop
//{
//    public class CustomInterceptor : AbstractInterceptor
//    {
//        public AdminUserService userService { get; set; }
//        public CustomInterceptor(AdminUserService adminUserService)
//        {
//            userService = adminUserService;
//        }
//        //[FromServiceContext]
//        //public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
//        public override async Task Invoke(AspectContext context, AspectDelegate next)
//        {
//            try
//            {
//                //var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
//                if (!await userService.IsSupperAdmin())
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
