using System;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Filter;
using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 第三方账号
    /// </summary>
    [HiddenApi]
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "第三方账号相关")]
    public class Part3Controller : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BZAutho2Model> _thirdRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public Part3Controller(IUnitOfWork unitOfWork ,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _thirdRepository = unitOfWork.GetRepository<BZAutho2Model>(true);
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZOauthDto dto)
        {
            var user = _mapper.Map<BZAutho2Model>(dto);
             await _thirdRepository.InsertAsync(user);
            return Ok();
        }



        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}/{oprationId}")]
        public IActionResult Delete([FromRoute] string Id, string oprationId)
        {
            _thirdRepository.LogicDelete(Id, oprationId);
            return Ok();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZOauthDto Dto )
        {
            if (!Guid.TryParse(Dto.Id,out _) )
                return new BadRequestResponse("id is error");
            var user = _mapper.Map<BZAutho2Model>(Dto);

            _thirdRepository.Update(user);
            return Ok();
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] string Id)
        {
            var res = await _thirdRepository.FindAsync(Id);
            if ( res is null )
                    return NoContent( );
            return Ok(res);
        }
     
    }
}