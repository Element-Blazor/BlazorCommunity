﻿using BlazorCommunity.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Features.Accounts.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<BZUserModel> _userManager;
        private readonly SignInManager<BZUserModel> _signInManager;
        private readonly IDataProtector _dataProtector;

        public AccountController(IDataProtectionProvider dataProtectionProvider, UserManager<BZUserModel> userManager, SignInManager<BZUserModel> signInManager)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("SignIn");
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [HttpGet("account/signinactual")]
        public async Task<IActionResult> SignInActual(string t)
        {
            var data = _dataProtector.Unprotect(t);

            var parts = data.Split('|');

            var identityUser = await _userManager.FindByIdAsync(parts[0]);
            var isTokenValid = await _userManager.VerifyUserTokenAsync(identityUser, TokenOptions.DefaultProvider, "SignIn", parts[1]);
            if (isTokenValid)
            {
                await _signInManager.SignInAsync(identityUser, true);
                if (parts.Length == 3 && Url.IsLocalUrl(parts[2]))
                {
                    return Redirect(parts[2]);
                }
                return Redirect("/");
            }
            else
            {
                return Unauthorized("STOP!");
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("account/signout")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("account/signout2")]
        public async Task<IActionResult> SignOut2()
        {
            await _signInManager.SignOutAsync();

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["returnUrl"]))
                return Redirect(HttpContext.Request.Query["returnUrl"]);
            return Redirect("/account/signin");
        }
    }
}