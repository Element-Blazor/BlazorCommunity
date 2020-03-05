using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Service
{
    public interface ICodeService
    {
        /// <summary>
        /// 产生指定长度的纯数字字符串
        /// </summary>
        /// <param name="length">最大8，最小4</param>
        /// <returns></returns>
        string GenerateNumberCode(int length = 6);
        /// <summary>
        /// 产生指定长度的字母+数字的字符串
        /// </summary>
        /// <param name="length">最大8，最小4</param>
        /// <returns></returns>
        string GenerateNumberLetterCode(int length = 4);
    }
}
