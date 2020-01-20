using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using BlazUICommunity.DTO;
using BlazUICommunity.Model.Models;
using BlazUICommunity.Request;
using BlazUICommunity.Utility.Extensions;
using BlazUICommunity.Utility.Response;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace BlazUICommunity.Api.Controllers
{
    /// <summary>
    /// 地址
    /// </summary>
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "地区相关")]
    public class RegionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BZAddressModel> _addressRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public RegionController(IUnitOfWork unitOfWork ,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _addressRepository = unitOfWork.GetRepository<BZAddressModel>(true);
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZAddressDto dto)
        {
            var user = _mapper.Map<BZAddressModel>(dto);
             await _addressRepository.InsertAsync(user);
            return Ok();
        }



        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            _addressRepository.Delete(Id);
            return Ok();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZAddressDto Dto , [FromRoute] int Id)
        {
            if ( Id < 1 )
                return new BadRequestResponse("id is error");
            var user = _mapper.Map<BZAddressModel>(Dto);
            user.Id = Id;

            _addressRepository.Update(user);
            return Ok();
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] int Id)
        {
            var res = await _addressRepository.FindAsync(Id);
            if ( res is null )
                return new NoContentResponse();
            return Ok(res);
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] AddressRequest Request = null)
        {
            IPagedList<BZAddressModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZAddressModel , AddressRequest>();
            pagedList = query == null ? await _addressRepository.GetPagedListAsync(Request.pageInfo.PageIndex - 1 , Request.pageInfo.PageSize) :
                       await _addressRepository.GetPagedListAsync(query , o => o.OrderBy(p => p.Id) , null , Request.pageInfo.PageIndex - 1 , Request.pageInfo.PageSize);
            return Ok(pagedList);
        }
    }
}