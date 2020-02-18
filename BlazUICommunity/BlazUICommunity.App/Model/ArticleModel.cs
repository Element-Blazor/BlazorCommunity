using Blazui.Community.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Model
{
    public class ArticleModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public TopicType TopicType { get; set; }
        public int versionId { get; set; }
        public ProjectType Project { get; set; }
        //public string VerName { get; set; }
        public string VerNo { get; set; }
    }

    public class ReplyModel
    {
        public string Content { get; set; }
    }
   

    public enum ProjectType
    {
        [Description("Blazui")]
        Blazui,
        [Description("BAdmin")]
        BAdmin,
        [Description("BMarkdown")]
        BMarkdown
    }
}
