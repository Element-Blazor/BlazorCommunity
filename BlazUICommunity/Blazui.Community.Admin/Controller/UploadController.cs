using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Filter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blazui.Community.App.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            _hostingEnvironment = webHostEnvironment;
        }

        [HttpPost("Banner")]
        public async Task<IActionResult> Upload(IFormFile fileContent)
        {
            try
            {
                var path = _hostingEnvironment.WebRootPath + "/img/banner/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                await Task.Delay(new Random().Next(500));

                if (ExistsFile(fileContent, out string filename))
                {
                    return OkResult(filename, path);
                }
                string fileExt = Path.GetExtension(fileContent.FileName); //文件扩展名
                //long fileSize = fileContent.Length; //获得文件大小，以字节为单位
                var fileId = Guid.NewGuid().ToString();
                string newFileName = fileId + fileExt; //随机生成新的文件名

                var filePath = Path.Combine(path, newFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await fileContent.CopyToAsync(stream);
                return OkResult(newFileName, "/img/banner/");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
     
   
        private IActionResult OkResult(string newFileName, string path)
        {
            return Content(JsonConvert.SerializeObject(new
            {
                //0表示成功
                code = 0,
                //id为文件唯一标识符
                id = path + newFileName,
                url = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + path + newFileName
            }), "application/json");
        }

        /// <summary>
        /// 是否存在与本次上传文件内容完全一致的文件，若有则直接返回已有的文件名，不重复上传
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool ExistsFile(IFormFile formFile, out string filePath)
        {
            filePath = "";
            var files = Directory.GetFiles(_hostingEnvironment.WebRootPath + filePath);
            using var uploadFile = formFile.OpenReadStream();
            foreach (var file in files)
            {
                using var filestream = new FileStream(file, FileMode.Open);
                if (uploadFile.CompareByReadOnlySpan(filestream))
                {
                    filePath = Path.GetFileName(file);
                    return true;
                }
            }
            return false;
        }
    }
}