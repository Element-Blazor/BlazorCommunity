using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blazui.Community.App.Features.Seo
{
    public class BlazorController : Controller
    {
        private readonly BZTopicRepository _bZTopicRepository;
        public BlazorController(BZTopicRepository bZTopicRepository)
        {
            _bZTopicRepository = bZTopicRepository;
        }
        [HttpGet("/archive")]
        [HttpGet("/archive/{pageIndex}/{pageSize}")]
        public  async Task<IActionResult> Index(int pageIndex=0,int pageSize=10)
        {
            var result = await _bZTopicRepository.GetPagedListAsync(p=>p.Status!=-1,null,null, pageIndex, pageSize);
            return View("/Features/Seo/Archive.cshtml", result);
        }

        [Route("/topic_seo/{id}")]
        [HttpGet]
        public IActionResult Topic(string id)
        {
            var model = _bZTopicRepository.Find(id);
            return View("/Features/Seo/Topic.cshtml", model);
        }
    }
}