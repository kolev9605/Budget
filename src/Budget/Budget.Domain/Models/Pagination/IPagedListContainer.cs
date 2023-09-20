using System.Collections.Generic;

namespace Budget.Domain.Models.Pagination;

public interface IPagedListContainer<T>
{
    IEnumerable<T> Items { get; }

    int PageNumber { get; }

    int TotalPages { get; }

    bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}
