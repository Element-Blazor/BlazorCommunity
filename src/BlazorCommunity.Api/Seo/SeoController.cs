using System.Threading.Tasks;
using AutoMapper;
using BlazorCommunity.Api.Service;
using BlazorCommunity.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BlazorCommunity.MvcCore;
using Markdig;
using System.Web;

namespace BlazorCommunity.Api.Seo
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
            topic.Content = HttpUtility.HtmlDecode(Markdig.Markdown.ToHtml(topic.Content, pipeline));
            topic.Title = HttpUtility.HtmlDecode(topic.Title);
            return View("/Seo/Topic.cshtml", topic);
           
        }
    }
}