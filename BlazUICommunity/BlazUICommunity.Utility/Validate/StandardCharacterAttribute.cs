using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Blazui.Community.Utility.Validate
{
    public class StandardCharacterAttribute : ValidationAttribute
    {

        private string pattern;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        public StandardCharacterAttribute(string pattern)
        {
            this.pattern = pattern;
        }

        /// <summary>
        /// 格式错误返回消息 非时间格式会触发 本例不需要重写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            return true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return base.IsValid(value, validationContext);
            if (Regex.IsMatch(value.ToString(), this.pattern))
            {
                return base.IsValid(value, validationContext);
            }
            else return new ValidationResult("字符串格式不正确");
        }
    }
}
