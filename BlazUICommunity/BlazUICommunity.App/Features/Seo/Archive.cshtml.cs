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
    public class ArchiveModel : PageModel
    {
        private readonly BZTopicRepository _bZTopicRepository;
        public ArchiveModel(BZTopicRepository bZTopicRepository)
        {
            _bZTopicRepository = bZTopicRepository;
        }

        public List<BZTopicModel> Topics { get; private set; }

        public void OnGet()
        {
            Topics = _bZTopicRepository.GetAll().ToList();
        }
    }
}
