using Blazui.Community.Enums;
using Blazui.Community.Utility.Extensions;
using Blazui.Component;
using System;

namespace Blazui.Community.DTO.Admin
{
   public class VersionDisplayDto
    {

        [TableColumn(Ignore = true)]
        public string Id { get; set; }

        /// <summary>
        /// 0：blazui，1：bAdmin，2：BMarkdown
        /// </summary>
        [TableColumn(Ignore = true)]
        public ProjectType Project { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [TableColumn(Text = "版本编号")]
        public string VerNo { get; set; }
        /// <summary>
        /// 版本名称
        /// </summary>
        [TableColumn(Text = "版本名称")]
        public string VerName { get; set; }
        /// <summary>
        /// 版本更新说明
        /// </summary>
        [TableColumn(Text = "版本说明")]
        public string VerDescription { get; set; }
        /// <summary>
        /// 版本发布日期
        /// </summary>
        [TableColumn(Text = "发布日期", Format = "yyyy-MM-dd")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 最后修改日期
        /// </summary>
        [TableColumn(Ignore = true)]
        public DateTime LastModifyDate { get; set; }
        /// <summary>
        /// 版本nuget包地址
        /// </summary>
        [TableColumn(Text = "Nuget")]
        public string VerNuget { get; set; }
        /// <summary>
        /// 版本下载地址
        /// </summary>
        [TableColumn(Text = "下载地址")]
        public string VerDownUrl { get; set; }

        /// <summary>
        /// 文档地址
        /// </summary>
        [TableColumn(Text = "文档地址")]
        public string VerDocUrl { get; set; }
        /// <summary>
        /// -1已删除，已废弃，0-正常
        /// </summary>
        [TableColumn(Ignore = true)]
        public int Status { get; set; }
        [AutoNotMap]
        [TableColumn(Text = "状态", Width = 60)]
        public string StatusDisplay => ((DelStatus)Status).Description();

        [AutoNotMap]
        [TableColumn(Text = "项目", Width = 100)]
        public string ProjectDisplay => Project.Description();
    }
}
