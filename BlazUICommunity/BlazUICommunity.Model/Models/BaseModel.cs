using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazUICommunity.Model
{
   public  class BaseModel
    {
        /// <summary>
        /// 主键，自增
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}
