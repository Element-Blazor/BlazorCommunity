using BlazorCommunity.App.Model.Condition;
using BlazorCommunity.App.Pages;
using BlazorCommunity.DTO;
using BlazorCommunity.Model.Models;
using Blazui.Component;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCommunity.App.Components.User;

namespace BlazorCommunity.App.Components
{
    public class PersonalTopicFollowedBase : PersonalTopicTableBase<PersonalFollowDisplayDto>
    {


        /// <summary>
        /// 调用webapi接口获取数据，转换数据，加载界面
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected override async Task LoadDatas()
        {
            var result = await NetService.QueryFollows(CreateCondition());
            if (result == null)
                return;
            SetData(result.Data?.Items, result.Data?.TotalCount);


        }
        /// <summary>
        /// 删除纪录
        /// </summary>
        /// <param name="topic"></param>
        public async Task Del(object topic)
        {
            MessageBoxResult Confirm = await MessageBox.ConfirmAsync("确定要取消收藏？");
            if (Confirm == MessageBoxResult.Ok)
                if (topic is PersonalFollowDisplayDto followModel)
                {
                    var result = await NetService.CancelFollow(followModel.FollowId);
                    if (result.IsSuccess)
                    {
                        await SearchData();
                        ToastWarning("取消收藏了");
                    }
                }
        }

        private SearchPersonalFollowCondition CreateCondition()
        {
            var condition = searchForm.GetValue<SearchPersonalFollowCondition>();
            condition ??= new SearchPersonalFollowCondition();
            condition.CreatorId = User.Id;
            condition.PageSize = pageSize;
            condition.PageIndex = currentPage;
            return condition;
        }

        protected void LinktoTopic(object topic)
        {
            if (topic is PersonalFollowDisplayDto topicModel)
                base.LinktoTopic(topicModel.Id);
        }
    }
}