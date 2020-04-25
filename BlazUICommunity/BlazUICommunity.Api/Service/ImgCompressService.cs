using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Blazui.Community.Api.Service
{
    public class ImgCompressService
    {

        private readonly string TinypngApiUrl = "https://api.tinify.com/shrink";
        private readonly string Key = string.Empty;
        private readonly string WaterMarkPngPath = string.Empty;
        private readonly string tempupload = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iconfiguration"></param>
        /// <param name="webHostEnvironment"></param>
        public ImgCompressService(IConfiguration iconfiguration, IWebHostEnvironment webHostEnvironment)
        {
            Key = iconfiguration.GetSection("TinypngKey").Value;
            WaterMarkPngPath = Path.Combine(webHostEnvironment.WebRootPath, "watermark.png");
            tempupload = Path.Combine(webHostEnvironment.WebRootPath,"upload", "temp");
        }

        public async Task Compress(string filePath, string filename)
        {
            try
            {
                using var imagesTemle = Image.Load(filePath);
                using var outputImg = Image.Load(WaterMarkPngPath);
                outputImg.Mutate(a => a.Resize(new SixLabors.Primitives.Size(imagesTemle.Width / 2, imagesTemle.Height / 2)));
                //进行多图片处理
                imagesTemle.Mutate(operation => operation.DrawImage(outputImg, new SixLabors.Primitives.Point(imagesTemle.Width / 4 + outputImg.Width / 4, imagesTemle.Height / 3), 0.5f));
                imagesTemle.Save(filePath);

                WebClient client = new WebClient();
                string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes("api:" + Key));
                client.Headers.Add(HttpRequestHeader.Authorization, "Basic " + auth);

                await client.UploadDataTaskAsync(TinypngApiUrl, File.ReadAllBytes(filePath));
                await client.DownloadFileTaskAsync(client.ResponseHeaders["Location"], Path.Combine(tempupload,filename));
                File.Move(Path.Combine(tempupload, filename), filePath,true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("网络请求失败:" + filePath);
                Console.WriteLine("网络请求失败:" + ex.StackTrace);
            }
        }
    }
}
