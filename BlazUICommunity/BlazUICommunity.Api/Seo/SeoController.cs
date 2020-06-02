using System.Threading.Tasks;
using AutoMapper;
using Blazui.Community.Api.Service;
using Blazui.Community.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Blazui.Community.MvcCore;
using Markdig;

namespace Blazui.Community.Api.Seo
{
    [IgnoreApiResultAttribute]
    public class SeoController : Controller
    {
        private readonly BZTopicRepository bZTopicRepository;
        private readonly ICacheService cacheService;
        public IMapper Mapper { get; }

        public SeoController(BZTopicRepository bZTopicRepository, IMapper mapper, ICacheService cacheService)
        {
            this.bZTopicRepository = bZTopicRepository;
            Mapper = mapper;
            this.cacheService = cacheService;
        }


        [HttpGet("/archive")]
        [HttpGet("/archive/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> Index(int pageIndex = 0, int pageSize = 10)
        {
            var result = await bZTopicRepository.GetPagedListAsync(p=>p.Status!=-1,o=>o.OrderByDescending(x=>x.CreateDate),null,pageIndex, pageSize);
            return View("/Seo/Archive.cshtml", result);
        }

        [Route("/topic_seo/{id}")]
        [HttpGet]
        public async Task<IActionResult> Topic(string id)
        {

            var topics = await cacheService.GetTopicsAsync(p => p.Id == id);

            var topic = topics?.FirstOrDefault();
            if (topic == null) return View("/Seo/Topic.cshtml");
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions().UseAutoLinks()
                .Build();
            topic.Content = Markdig.Markdown.ToHtml(topic.Content, pipeline);
            return View("/Seo/Topic.cshtml", topic);
           
        }
    }
}