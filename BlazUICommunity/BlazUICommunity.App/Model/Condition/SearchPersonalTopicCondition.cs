using Blazui.Community.Utility.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Model.Condition
{

    public class SearchPersonalTopicCondition: BaseSearchCondition
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public TopicType? TopicType { get; set; }

    }

  
}
