using System;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Filter;
using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 地址
    /// </summary>
    [HiddenApi]
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
            var addressModel = _mapper.Map<BZAddressModel>(dto);
             await _addressRepository.InsertAsync(addressModel);
            return Ok();
        }



        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}/{oprationId}")]
        public IActionResult Delete([FromRoute] string Id, string oprationId)
        {
            _addressRepository.LogicDelete(Id, oprationId);
            return Ok();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZAddressDto Dto)
        {
            if ( !Guid.TryParse(Dto.Id,out _) )
                return new BadRequestResponse("id is error");
            var addressModel = _mapper.Map<BZAddressModel>(Dto);

            _addressRepository.Update(addressModel);
            return Ok();
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] string Id)
        {
            var res = await _addressRepository.FindAsync(Id);
            if ( res is null )
                    return NoContent( );
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
            pagedList = query == null ? await _addressRepository.GetPagedListAsync(Request.PageInfo.PageIndex - 1 , Request.PageInfo.PageSize) :
                       await _addressRepository.GetPagedListAsync(query , o => o.OrderBy(p => p.Id) , null , Request.PageInfo.PageIndex - 1 , Request.PageInfo.PageSize);
            return Ok(pagedList);
        }
    }
}