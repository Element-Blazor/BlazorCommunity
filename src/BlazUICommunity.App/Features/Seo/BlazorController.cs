using System.Threading.Tasks;
using Blazui.Community.App.Service;
using Microsoft.AspNetCore.Mvc;

namespace Blazui.Community.App.Features.Seo
{
    public class BlazorController : Controller
    {
        private readonly NetworkService networkService;
        public BlazorController(NetworkService service)
        {
            networkService = service;
        }
        [HttpGet("/archive")]
        [HttpGet("/archive/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 10)
        {
            var result = await networkService.QueryTopicsWithPage(pageIndex, pageSize);
            return View("/Features/Seo/Archive.cshtml", result.Data);
        }

        [Route("/topic_seo/{id}")]
        [HttpGet]
        public async Task<IActionResult> Topic(string id)
        {
            var model = await networkService.QueryTopicById(id);
            return View("/Features/Seo/Topic.cshtml", model?.Data);
        }
    }
}