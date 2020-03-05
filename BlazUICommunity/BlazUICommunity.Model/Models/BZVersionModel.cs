using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blazui.Community.Model.Models
{

    [Table("BZVersion")]
  public  class BZVersionModel:BaseModel
    {
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
    }
}
