using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Community.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blazui.Community.App.Features.Seo
{
    public class BlazorController : Controller
    {
        private readonly BZTopicRepository _bZTopicRepository;
        public BlazorController(BZTopicRepository bZTopicRepository)
        {
            _bZTopicRepository = bZTopicRepository;
        }

        [Route("/archive")]
        [HttpGet]
        public IActionResult Index()
        {
            return View("/Features/Seo/Archive.cshtml"/*, _bZTopicRepository.GetAll().ToList()*/);
        }

        [Route("/topic_seo/{id}")]
        [HttpGet]
        public IActionResult Topic(string id)
        {
            return View("/Features/Seo/Topic.cshtml"/*, _bZTopicRepository.Find(id)*/);
        }
    }
}