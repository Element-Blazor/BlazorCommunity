using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using Blazui.Community.DTO;
using System.Collections.Generic;

namespace Blazui.Community.Api.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class PageExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="pagedList"></param>
        /// <returns></returns>
        public static PageDatas<K> ConvertToPageData<T, K>(this IPagedList<T> pagedList)
        {
            PageDatas<K> pageDatas = new PageDatas<K>
            {
                IndexFrom = pagedList.IndexFrom,
                PageIndex = pagedList.PageIndex,
                PageSize = pagedList.PageSize,
                TotalCount = pagedList.TotalCount,
                TotalPages = pagedList.TotalPages
            };
            pageDatas.Items = new List<K>();
            return pageDatas;
        }
    }
}
