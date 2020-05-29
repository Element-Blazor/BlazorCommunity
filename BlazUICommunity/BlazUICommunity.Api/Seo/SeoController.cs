using System.Threading.Tasks;
using AutoMapper;
using Blazui.Community.Api.Service;
using Blazui.Community.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Blazui.Community.MvcCore;
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
            if (topics.Any())
            {
                var topic = topics.FirstOrDefault();
                if (topic.Status == -1)
                    return NoContent();
                if (topic.Status == 0)//已结帖不再更新浏览量
                {
                    topic.Hot++;
                    bZTopicRepository.Update(topic);
                }
                var topicDto = Mapper.Map<DTO.BZTopicDto>(topic);
                var user = (await cacheService.GetUsersAsync(p => p.Id == topic.CreatorId)).FirstOrDefault();
                var version = (await cacheService.GetVersionsAsync(p => p.Id == topic.Id)).FirstOrDefault();

                topicDto.UserName = user?.UserName;
                topicDto.NickName = user?.NickName;
                topicDto.Avator = user?.Avator;
                topicDto.VerName = version?.VerName;
                topicDto.Signature = user.Signature;

            }
            else
            {
                var res = await bZTopicRepository.QueryTopById(id);
                if (res is null)
                    return NoContent();

            }
            return View("/Seo/Topic.cshtml", topics?.FirstOrDefault());
        }
    }
}