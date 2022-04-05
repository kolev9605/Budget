using System;
using System.Collections.Generic;

namespace Budget.Core.Models.Pagination
{
    public class PaginationModel<T>
    {
        public PaginationModel(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            Items.AddRange(items);
        }

        private PaginationModel()
        {

        }

        public List<T> Items { get; private set; } = new List<T>();

        public int PageNumber { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public PaginationModel<TResult> Convert<TResult>(PaginationModel<T> paginationModel, List<TResult> items)
        {
            return new PaginationModel<TResult>()
            {
               Items = items,
               PageNumber = paginationModel.PageNumber,
               TotalPages = paginationModel.TotalPages,
            };
        }
    }
}
