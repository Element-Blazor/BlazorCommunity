using System.Collections.Generic;

namespace Blazui.Community.DTO
{

    public class PageDatas<T>
    {
        /// <summary>
        /// Gets the index start value.
        /// </summary>
        /// <value>The index start value.</value>
        public int IndexFrom { get; set; }
        /// <summary>
        /// Gets the page index (current).
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets the total count of the list of type <typeparamref name="T"/>
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// Gets the current page items.
        /// </summary>
        public IList<T> Items { get; set; }
        /// <summary>
        /// Gets the has previous page.
        /// </summary>
        /// <value>The has previous page.</value>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Gets the has next page.
        /// </summary>
        /// <value>The has next page.</value>
        public bool HasNextPage { get; set; }
    }
}
