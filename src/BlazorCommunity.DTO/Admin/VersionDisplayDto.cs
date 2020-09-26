using BlazorCommunity.AutoMapperExtensions;
using BlazorCommunity.Enums;
using System;

namespace BlazorCommunity.DTO.Admin
{
    public class VersionDisplayDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 0：blazui，1：bAdmin，2：BMarkdown
        /// </summary>
        public int Project { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string VerNo { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        public string VerName { get; set; }

        /// <summary>
        /// 版本更新说明
        /// </summary>
        public string VerDescription { get; set; }

        /// <summary>
        /// 版本发布日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 最后修改日期
        /// </summary>
        public DateTime LastModifyDate { get; set; }

        /// <summary>
        /// 版本nuget包地址
        /// </summary>
        public string VerNuget { get; set; }

        /// <summary>
        /// 版本下载地址
        /// </summary>
        public string VerDownUrl { get; set; }

        /// <summary>
        /// 文档地址
        /// </summary>
        public string VerDocUrl { get; set; }

        /// <summary>
        /// -1已删除，已废弃，0-正常
        /// </summary>
        public int Status { get; set; }

        [AutoNotMap]
        public string StatusDisplay => ((DelStatus)Status).Description();

        //[AutoNotMap]
        //public string ProjectDisplay => Project.Description();
    }
}