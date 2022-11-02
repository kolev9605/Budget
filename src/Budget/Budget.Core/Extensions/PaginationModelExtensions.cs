using Budget.Core.Models.Pagination;
using System.Collections.Generic;

namespace Budget.Core.Extensions
{
    public static class PaginationModelExtensions
    {
        public static PaginationModel<V> Convert<T, V>(this PaginationModel<T> paginationModel, List<V> items)
        {
            return paginationModel.Convert(paginationModel, items);
        }
    }
}
