﻿using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.LinqExtensions;
using BlazorCommunity.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlazorCommunity.Api.Controllers.Client
{   /// <summary>
    ///
    /// </summary>
    [Route("api/client/[controller]")]
    [ApiController]
    [SwaggerTag(description: "版本信息")]
    public class VersionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        ///
        /// </summary>
        /// <param name="unitOfWork"></param>
        public VersionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Project}")]
        public async Task<IActionResult> Get([SwaggerParameter(Required = false)]int Project = -1)
        {
            Expression<Func<BZVersionModel, bool>> expression = p => p.Status == 0;
            if (Project != -1)
                expression = expression.And(p => p.Project == Project);
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            var versions = await verRepository.GetAllAsync(expression);

            return Ok(versions);
        }

        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            Expression<Func<BZVersionModel, bool>> expression = p => p.Status == 0;
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            var versions = await verRepository.GetAllAsync(expression);
            return Ok(versions);
        }
    }
}