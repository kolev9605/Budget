using System;
using System.Collections.Generic;

namespace Budget.Domain.Models.Pagination;

public class PagedListContainer<T> : IPagedListContainer<T>
{
    public PagedListContainer(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        Items = new List<T>(items);
    }

    public IEnumerable<T> Items { get; init; } = new List<T>();

    public int PageNumber { get; init; }

    public int TotalPages { get; init; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}
