using Blazui.Community.Enums;
using Blazui.Community.FileExtensions;
using Blazui.Community.MvcCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using static Blazui.Community.Api.Configuration.ConstantConfiguration;

namespace Blazui.Community.Api.Controllers
{
    [EnableCors(PolicyName)]
    [Route("api/[controller]")]
    [ApiController]
    [BlazuiUploadApiResult]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            _hostingEnvironment = webHostEnvironment;
        }

        [HttpPost(nameof(UploadPath.Avator))]
        public async Task<IActionResult> Avator([FromForm]IFormFile fileContent)
        {
            return await Upload(UploadPath.Avator, fileContent);
        }

        [HttpPost(nameof(UploadPath.Banner))]
        public async Task<IActionResult> Banner([FromForm]IFormFile fileContent)
        {
            return await Upload(UploadPath.Banner, fileContent);
        }

        [HttpPost(nameof(UploadPath.Topic))]
        public async Task<IActionResult> Topic([FromForm]IFormFile fileContent)
        {
            return await Upload(UploadPath.Topic, fileContent);
        }

        [HttpPost(nameof(UploadPath.Other))]
        public async Task<IActionResult> Other([FromForm]IFormFile fileContent)
        {
            return await Upload(UploadPath.Other, fileContent);
        }

        private async Task<IActionResult> Upload(UploadPath Upload, IFormFile fileContent)
        {
            try
            {
                if (fileContent is null)
                    return BadRequest();
                var FileSaveFolder = Path.Combine(UploadRootPath, Upload.Description());
                var FileSaveFullFolder = Path.Combine(_hostingEnvironment.WebRootPath, FileSaveFolder);

                ExistsFile(fileContent, FileSaveFullFolder, out string ExistsFileName);
                if (!string.IsNullOrWhiteSpace(ExistsFileName))
                    return Success(Upload, ExistsFileName);

                string FileName = Guid.NewGuid().ToString() + Path.GetExtension(fileContent.FileName);
                var SavePath = Path.Combine(FileSaveFullFolder, FileName);
                using var stream = new FileStream(SavePath, FileMode.Create);
                await fileContent.CopyToAsync(stream);
                return Success(Upload, FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Upload"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private IActionResult Success(UploadPath Upload, string FileName)
        {
            var UploadResult = new
            {
                //0表示成功
                code = 0,
                //id为文件唯一标识符
                id = FileName,
                //最终文件访问路径
                url = $"{ HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{UploadRootPath}/{Upload.Description()}/{FileName}"
            };

            return Content(JsonConvert.SerializeObject(UploadResult), "application/json");
        }

        /// <summary>
        /// 检查是否已存在文件
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="FullFileSaveFolder"></param>
        /// <param name="filePath"></param>
        private void ExistsFile(IFormFile formFile, string FullFileSaveFolder, out string filePath)
        {
            filePath = "";
            var files = Directory.GetFiles(FullFileSaveFolder);
            using var uploadFile = formFile.OpenReadStream();
            foreach (var file in files)
            {
                using var filestream = new FileStream(file, FileMode.Open);
                if (uploadFile.CompareByReadOnlySpan(filestream))
                {
                    filePath = Path.GetFileName(file);
                    break;
                }
            }
        }
    }
}