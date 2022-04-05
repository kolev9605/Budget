using System;
using System.Collections.Generic;

namespace Budget.Core.Models.Pagination
{
    public class PaginationModel<T>
    {
        public PaginationModel(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            Items.AddRange(items);
        }

        public List<T> Items { get; private set; } = new List<T>();

        public int PageIndex { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;
    }
}
