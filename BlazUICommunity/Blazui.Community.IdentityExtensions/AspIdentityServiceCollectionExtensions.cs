using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazui.Community.IdentityExtensions
{
    public static class AspIdentityServiceCollectionExtensions
    {
        public static void AddCustomAspIdenitty<TUser, TDbContext>(this IServiceCollection services) where TUser : IdentityUser<string> where TDbContext : DbContext
        {
            services.AddIdentity<TUser, IdentityRole<string>>(options =>
            {
                options.Password.RequiredLength = 8;//要求必须8以上位密码
                options.Password.RequireUppercase = false;//要求小写
                options.Password.RequireLowercase = false;//要求大写
                options.Password.RequireDigit = false;//要求必须有数字
                options.Password.RequireNonAlphanumeric = false;//要求有特殊字符
                options.SignIn.RequireConfirmedEmail = false;//要求必须要验证邮箱
                                                             // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);//登录失败锁定时间
                options.Lockout.MaxFailedAccessAttempts = 3;//重试次数
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.";//用户名允许出现的字符@.允许邮箱作为用户名
                options.User.RequireUniqueEmail = false;//邮箱验证

                options.SignIn = new SignInOptions
                {
                    RequireConfirmedEmail = false, //要求激活邮箱
                    RequireConfirmedPhoneNumber = false //要求激活手机号
                };
            }).AddErrorDescriber<CustomIdentityErrorDescriber>()
              .AddEntityFrameworkStores<TDbContext>()
              .AddDefaultTokenProviders();
        }
    }
}