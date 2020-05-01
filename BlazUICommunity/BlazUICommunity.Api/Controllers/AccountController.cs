using Blazui.Community.Api.Jwt;
using Blazui.Community.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Community.DateTimeExtensions;
using Blazui.Community.Common;
using Blazui.Community.Shared;
using System.Collections.Generic;

namespace Blazui.Community.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<BZUserModel> _userManager;
        private readonly SignInManager<BZUserModel> _signInManager;
        private readonly JwtService jwtService;

        public AccountController(
            UserManager<BZUserModel> userManager,
            SignInManager<BZUserModel> signInManager, 
            JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.jwtService = jwtService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var loginResult = new LoginResult { Errors = new List<string>(), Successful=false, Token=new TokenResult() { AccessToken="", Code=0, RefreshToken="", RefreshTokenExpired=DateTime.Now} };
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, false, false);

            if (!result.Succeeded)
            {
                loginResult.Errors = new List<string>()
                {
                    "用户名或密码错误"
                };
                return Ok(loginResult);
            }


            var user = await _userManager.FindByNameAsync(login.Username);
            if (user.Status == -1)
            {
                loginResult.Errors = new List<string>()
                {
                    "该账号当前被封禁"
                };
                return Ok(loginResult);
            }
            var role = (await _userManager.GetRolesAsync(user))?.FirstOrDefault()??"";

            var token = jwtService.GetAccessToken(new SessionUser() { Id = user.Id, Name = user.UserName, Role = role });

            var refreshToken = Guid.NewGuid().ToString("N");

            var refreshTokenExpiredTime = DateTime.Now.AddMinutes(60);

            loginResult.Token= new TokenResult
            {
                AccessToken = token,
                Code = 200,
                RefreshTokenExpired = refreshTokenExpiredTime,
                RefreshToken = refreshToken
            };
            loginResult.Successful = true;
            return Ok(loginResult);
        }


        [HttpPost("Regist")]
        public async Task<IActionResult> Regist([FromBody]RegisterModel model)
        {
            var identityResult = await _userManager.CreateAsync(
                   new BZUserModel
                   {
                       UserName = model.Account,
                       NickName = model.NickName ?? model.Account,
                       Email = RegexHelper.IsEmail(model.Account) ? model.Account : model.Email ?? "",
                       EmailConfirmed = false,
                       NormalizedUserName = model.Account,
                       CreateDate = DateTime.Now,
                       LastLoginDate = DateTime.Now,
                       CreatorId = Guid.Empty.ToString(),
                       Sex = model.Sex,
                       Status = 0,
                       Avator = "/img/defaultAct.png",
                       PhoneNumber = model.Mobile ?? ""
                   }, model.Password);
            if (identityResult.Succeeded)
                return Ok(new RegisterResult { Successful = identityResult.Succeeded,Errors=new List<string>() });
            else
            {
                return Ok(new RegisterResult { Successful = false, Errors = identityResult.Errors.Select(x => x.Description) });
            }
        }
    }
}