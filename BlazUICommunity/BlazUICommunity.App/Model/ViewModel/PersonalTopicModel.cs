using Blazui.Community.Enums;
using Blazui.Component;
using System;

namespace Blazui.Community.App.Model
{
    public class PersonalTopicModel
    {
        [TableColumn(Ignore = true)]
        public string Id { get; set; }

        /// <summary>
        /// 帖子标题
        /// </summary>
        [TableColumn(Text = "标题", Width = 500)]
        public string Title { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        [TableColumn(Text = "发表时间", Format = "yyyy-MM-dd", Width = 100)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///  状态 0正常，-1 删除，1已结帖
        /// </summary>
        [TableColumn(Text = "状态", Ignore = true)]
        public int Status { get; set; }

        /// <summary>
        ///  状态 0正常，-1 删除，1已结帖
        /// </summary>
        [TableColumn(Text = "状态", Width = 60)]
        public string StatusDisplay
        {
            get
            {
                return ((DelStatus)Status).Description();
            }
        }

        /// <summary>
        /// 主题帖类型 0：提问，1：分享，2：讨论，3：建议，4：公告
        /// </summary>
        [TableColumn(Text = "类型", Ignore = false, Width = 50)]
        public string CategoryDisplay
        {
            get
            {
                return Category.Description();
            }
        }

        [TableColumn(Text = "类型", Ignore = true)]
        public TopicCategory Category { get; set; }

        /// <summary>
        /// 是否精华
        /// </summary>
        [TableColumn(Text = "精华", Ignore = true)]
        public int Good { get; set; }

        /// <summary>
        /// 是否精华
        /// </summary>
        [TableColumn(Text = "精华", Width = 50)]
        public string GoodDisplay
        {
            get
            {
                return Good switch
                {
                    0 => "否",
                    1 => "是",
                    _ => "否"
                };
            }
        }

        /// <summary>
        /// 回帖数量
        /// </summary>
        [TableColumn(Text = "回帖", Width = 50)]
        public int ReplyCount { get; set; }

        [TableColumn(Text = "时间", Ignore = true)]
        public string CreateTimeDisplay { get; set; }

        [TableColumn(Ignore = true)]
        public string UserName { get; set; }

        [TableColumn(Ignore = true)]
        public string NickName { get; set; }
    }
}