using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.App
{
    public class ConstConfig
    {
        /// <summary>
        /// 0-light 1-success 2-info 3-warning 4-danger
        /// </summary>
        public static string[] TagClasses = { "el-tag--light", "el-tag--success", "el-tag--info", "el-tag--warning", "el-tag--danger" };


        public static int width;
        public static int height;
        public static int MaxUploadFileSize = 1024;
    }
}
