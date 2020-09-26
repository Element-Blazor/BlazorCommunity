using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using BlazorCommunity.Api.Service;
using BlazorCommunity.DTO.Admin;
using BlazorCommunity.Model.Models;
using BlazorCommunity.Repository;
using BlazorCommunity.Request;
using BlazorCommunity.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorCommunity.Api.Controllers
{
    /// <summary>
    ///
    /// </summary>
    //[HiddenApi]
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "用户相关")]
    //[HttpCacheExpiration(MaxAge = 100)]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BZUserRepository _bZUserRepository;
        private readonly UserManager<BZUserModel> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly ICacheService _cacheService;
        /// <summary>
        ///
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="bZUserRepository"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="cacheService"></param>
        public UserController(IUnitOfWork unitOfWork,
            IMapper mapper,
            BZUserRepository bZUserRepository, UserManager<BZUserModel> userManager, RoleManager<IdentityRole<string>> roleManager, ICacheService cacheService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _bZUserRepository = bZUserRepository;
            _userManager = userManager;
            _cacheService = cacheService;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPatch("Delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            return await DeleteOrResume(Id, -1);
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <returns></returns>
        [HttpPatch("Resume/{Id}")]
        public async Task<IActionResult> Resume([FromRoute] string Id)
        {
            return await DeleteOrResume(Id, 0);
        }

        private async Task<IActionResult> DeleteOrResume(string Id, int Status)
        {
            var user = await _bZUserRepository.FindAsync(Id);
            if (user is null)
                return BadRequest();
            if (user.Status == Status)
                return Ok();
            user.Status = Status;
            user.LastModifyDate = DateTime.Now;
            user.LastModifierId = Guid.Empty.ToString();
            _bZUserRepository.Update(user);
            _cacheService.Remove(nameof(BZUserModel));
            return Ok();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpPatch("ResetPassword/{Id}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = new Random(DateTime.Now.Millisecond).Next(10000000, 99999999);//"88888888";//
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword.ToString());
            if (result.Succeeded)
                return Ok(newPassword);
            return new BadRequestResponse(JsonConvert.SerializeObject(result.Errors));
        }

        /// <summary>
        /// 根据条件分页查询用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query")]
        public async Task<IActionResult> Query([FromQuery] UsersRequestCondition Request = null)
        {
            var query = Request.CreateQueryExpression<BZUserModel, UsersRequestCondition>();
            var pagedList = await _bZUserRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.PageIndex - 1, Request.PageSize);
            if (pagedList.Items.Any())
            {
                var result = pagedList.From(result => _mapper.Map<List<UserDisplayDto>>(result));
                var roles = _roleManager.Roles;
                var userRoleRepository = _unitOfWork.GetRepository<IdentityUserRole<string>>();
                var useroles = await userRoleRepository.GetAllAsync();
                foreach (var item in result.Items)
                {
                    var userroles = useroles.Where(p => p.UserId == item.Id).Select(p=>p.RoleId);
                    var roleNames = roles.Where(p => userroles.Contains(p.Id)).Select(p => p.Name).ToList();
                    item.RoleName = string.Join(",", roleNames);
                }
                return Ok(result);
            }

            return NoContent();
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="newRoleDto"></param>
        /// <returns></returns>
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] NewRoleDto  newRoleDto)
        {
            var exist = await _roleManager.RoleExistsAsync(newRoleDto.Name);
            if (exist)
                return BadRequest();
            var createRoleResult = await _roleManager.CreateAsync(new IdentityRole { Name = newRoleDto.Name });
            if (createRoleResult.Succeeded)
                return Ok();
            else
                return BadRequest(createRoleResult.Errors);
        }


        /// <summary>
        /// 查询角色
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryRoles")]
        public IActionResult QueryRoles()
        {
            return Ok(_roleManager.Roles);
        }


        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="roleDisplayDto"></param>
        /// <returns></returns>
        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDisplayDto roleDisplayDto)
        {
            var role = await _roleManager.FindByIdAsync(roleDisplayDto.Id);
            if (role == null)
                return BadRequest("不存在");
            role.Name = roleDisplayDto.Name;

            var UpdateRoleResult = await _roleManager.UpdateAsync(role);
            if (UpdateRoleResult.Succeeded)
                return Ok();
            else
                return BadRequest(UpdateRoleResult.Errors);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRole/{RoleId}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null)
                return BadRequest("不存在");
            var DeleteRoleResult = await _roleManager.DeleteAsync(role);
            if (DeleteRoleResult.Succeeded)
                return Ok();
            else
                return BadRequest(DeleteRoleResult.Errors);
        }
        /// <summary>
        /// 添加角色声明
        /// </summary>
        /// <param name="roleClaimDto"></param>
        /// <returns></returns>
        [HttpPost("CreateRoleClaim")]
        public async Task<IActionResult> CreateRoleClaim([FromBody] RoleClaimDto roleClaimDto)
        {
            var role = await _roleManager.FindByIdAsync(roleClaimDto.RoleId);
            if (role != null)
            {
                var roleclaims = await _roleManager.GetClaimsAsync(role);
                if (roleclaims.FirstOrDefault(p => p.Type == roleClaimDto.ClaimType && p.Value == roleClaimDto.ClaimValue) != null)
                    return BadRequest("重复的声明");
                var addClaimResult = await _roleManager.AddClaimAsync(role, new Claim(roleClaimDto.ClaimType, roleClaimDto.ClaimValue));
                if (addClaimResult.Succeeded)
                    return Ok();
                else
                    return BadRequest(addClaimResult.Errors);
            }
            return BadRequest("角色不存在");
        }

        /// <summary>
        /// 根据RoleId查询Claims
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpGet("QueryRoleClaims/{RoleId}")]
        public async Task<IActionResult> QueryRoleClaims([FromRoute]string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if(role!=null)
            {
              var result=  await _roleManager.GetClaimsAsync(role);

                if (result != null)
                {
                    var ListRoleClaimDto = new List<RoleClaimDto>();
                    foreach (var item in result)
                    {
                        ListRoleClaimDto.Add(new RoleClaimDto { ClaimType=item.Type, RoleId=RoleId, ClaimValue=item.Value });
                    }

                    return Ok(ListRoleClaimDto);
                }
           
            }
            return BadRequest("role不存在");
        }
        /// <summary>
        /// 修改角色声明
        /// </summary>
        /// <param name="roleClaimDto"></param>
        /// <returns></returns>
        [HttpPost("DeleteRoleClaim")]
        public async Task<IActionResult> DeleteRoleClaim([FromBody] RoleClaimDto roleClaimDto)
        {
            var role = await _roleManager.FindByIdAsync(roleClaimDto.RoleId);
            if (role != null)
            {
                var roleclaims = await _roleManager.GetClaimsAsync(role);
                if (roleclaims.FirstOrDefault(p => p.Type == roleClaimDto.ClaimType && p.Value == roleClaimDto.ClaimValue) != null)
                {
                    var RemoveClaimResult = await _roleManager.RemoveClaimAsync(role, new Claim(roleClaimDto.ClaimType, roleClaimDto.ClaimValue));
                    if (RemoveClaimResult.Succeeded)
                    {
                        return Ok();
                    }
                    else
                        return BadRequest(RemoveClaimResult.Errors);
                }
                else
                    return BadRequest("声明不存在");

            }
            return BadRequest("角色不存在");
        }

        /// <summary>
        /// 为用户指定角色
        /// </summary>
        /// <param name="userRoleDto"></param>
        /// <returns></returns>
        [HttpPost("AddRolesToUser")]
        public async Task<IActionResult> AddRolesToUser([FromBody] UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByIdAsync(userRoleDto.UserId);
            if(user!=null)
            {

                var roles =await _userManager.GetRolesAsync(user);
                    var RemoveRoleResult = await _userManager.RemoveFromRolesAsync(user, roles);
                   if(RemoveRoleResult.Succeeded)
                {
                    if (userRoleDto.RoleIds != null && userRoleDto.RoleIds.Any())
                    {
                        var NewRoles = _roleManager.Roles.Where(p => userRoleDto.RoleIds.Contains(p.Id)).Select(p => p.Name).ToList();
                        var addToRoleResult = await _userManager.AddToRolesAsync(user, NewRoles);
                        if (addToRoleResult.Succeeded)
                        {
                            await _userManager.UpdateAsync(user); //完成储存
                            return Ok();
                        }
                        else
                            return BadRequest(addToRoleResult.Errors);
                    }
                    else
                    {
                        return Ok();
                    }
                }
                return BadRequest(RemoveRoleResult.Errors);
            }
            return BadRequest("用户不存在");
        }


        /// <summary>
        /// 根据用户ID查询角色
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet("QueryUserRoles/{UserId}")]
        public async Task<IActionResult> QueryUserRoles([FromRoute]string UserId)
        {

            var user = await _userManager.FindByIdAsync(UserId);
            if(user!=null)
            {
                var roles =await _userManager.GetRolesAsync(user);

                if (roles != null)
                {
                    return Ok(roles);
                }
            }
            return BadRequest("角色不存在");
        }

    }
}