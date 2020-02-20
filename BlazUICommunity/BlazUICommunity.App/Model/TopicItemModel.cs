using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Model
{
    public class TopicItemModel
    {


        public int Id { get; set; }
        /// <summary>
        /// 作者头像
        /// </summary>

        public string Avator { get; set; }
        /// <summary>
        /// 帖子标题
        /// </summary>

        public string Title { get; set; }
        /// <summary>
        /// 类别
        /// </summary>

        public string Category
        {
            get
            {
                return TopicType switch
                {
                    0 => "提问",
                    1 => "分享",
                    2 => "讨论",
                    3 => "建议",
                    4 => "公告",
                    _ => "UnKown"
                };
            }
        }


        public int TopicType { get; set; }
        /// <summary>
        /// 是否精华帖
        /// </summary>

        public bool IsBest { get; set; } = false;
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>

        public string ReleaseTime { get; set; }
        /// <summary>
        /// 回复数
        /// </summary>

        public int ReplyCount { get; set; }
        /// <summary>
        /// 0 正常 -1 已删除 1已结帖
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

    }
}
