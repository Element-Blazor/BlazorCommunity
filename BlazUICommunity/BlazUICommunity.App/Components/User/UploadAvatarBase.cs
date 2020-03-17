using Blazui.Component;
using Blazui.Component.Dom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class UploadAvatarBase : BUploadBase
    {
        internal ElementReference hdnField;

        internal HashSet<IFileModel> Files { get; set; } = new HashSet<IFileModel>();

        internal ElementReference Input { get; set; }

        internal async Task ScanFileAsync()
        {
            var input = Input.Dom(JSRuntime);
            var files = await input.ScanFilesAsync();
            ScanFiles(files);
            _ = UploadFilesAsync(input);
            RequireRender = true;
            SetFieldValue(Files.ToArray(), true);
        }

        private async Task UploadFilesAsync(Element input)
        {
            foreach (var item in Files)
            {
                var model = item as UploadModel;
                if (model.Status != UploadStatus.UnStart)
                {
                    continue;
                }
                if (OnFileUploadStart.HasDelegate)
                {
                    _ = OnFileUploadStart.InvokeAsync(item);
                }
                var results = await input.UploadFileAsync(item.FileName, Url);
                FileUploaded(model, results);
            }
            await input.ClearAsync();
            RequireRender = true;
            if (OnFileListUpload.HasDelegate)
            {
                _ = OnFileListUpload.InvokeAsync(Files.ToArray());
            }
            StateHasChanged();
        }

        private void FileUploaded(UploadModel model, string[] results)
        {
            if (results[0] == "0")
            {
                model.Status = UploadStatus.Success;
                if (OnFileUploadSuccess.HasDelegate)
                {
                    _ = OnFileUploadSuccess.InvokeAsync(model);
                }
            }
            else
            {
                model.Status = UploadStatus.Failure;
                if (OnFileUploadFailure.HasDelegate)
                {
                    _ = OnFileUploadFailure.InvokeAsync(model);
                }
            }
            model.Message = results[1];
            model.Id = results[2];
            model.Url = results[3];
        }

        protected override void OnParametersSet()
        {
            //base.OnParametersSet();
            AllowExtensions = AllowExtensions.Select(x => x?.Trim()).ToArray();
            if (FormItem == null)
            {
                return;
            }
            if (FormItem.OriginValueHasRendered)
            {
                return;
            }
            FormItem.OriginValueHasRendered = true;
            if (FormItem.Form.Values.Any())
            {
                InitilizeFiles(FormItem.OriginValue);
            }
        }

        private void InitilizeFiles(IFileModel[] fileModels)
        {
            Files = new HashSet<IFileModel>();
            if (fileModels == null)
            {
                return;
            }
            foreach (var item in fileModels)
            {
                Files.Add(new UploadModel()
                {
                    Url = item.Url,
                    FileName = item.FileName,
                    Id = item.Id,
                    Status = UploadStatus.Success
                });
            }
        }

        [Parameter]
        public int Limit { get; set; }

        private void ScanFiles(string[][] files)
        {
            if (Files.Count >= Limit)
            {
                if (Limit == 1)
                {
                    Files = new HashSet<IFileModel>();
                    var item = files[0];
                    AddFile(item);
                    return;
                }
                else
                {
                    Alert($"最多允许上传{Limit}个文件");
                    return;
                }
            }
            foreach (var item in files)
            {
                var ext = Path.GetExtension(item[0]);
                if (AllowExtensions.Any() && !AllowExtensions.Contains(ext, StringComparer.CurrentCultureIgnoreCase))
                {
                    Alert("您选择的文件中包含不允许上传的文件后缀");
                    return;
                }
                var size = Convert.ToInt64(item[1]);
                if (size / 1000 > MaxSize && MaxSize > 0)
                {
                    Alert("您选择的文件中包含大小超过允许大小的文件");
                    return;
                }
                if (item.Length >= 4)
                {
                    if ((Convert.ToInt32(item[2]) > Width && Width > 0) || (Convert.ToInt32(item[3]) > Height && Height > 0))
                    {
                        Alert("您选择的文件中包含尺寸超过允许大小的文件");
                        return;
                    }
                }
                AddFile(item);
            }
        }

        protected override void FormItem_OnReset(object value, bool requireRerender)
        {
            RequireRender = true;
            if (value == null)
            {
                Files = new HashSet<IFileModel>();
            }
            else
            {
                InitilizeFiles(value as IFileModel[]);
            }
            StateHasChanged();
        }

        private void AddFile(string[] item)
        {
            var file = new UploadModel()
            {
                FileName = Path.GetFileName(item[0]),
                Status = UploadStatus.UnStart,
                Url = item.Length == 5 ? item[4] : string.Empty
            };
            Files.Add(file);
        }
    }
}