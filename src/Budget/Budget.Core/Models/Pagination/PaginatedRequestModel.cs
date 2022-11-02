namespace Budget.Core.Models.Pagination
{
    public class PaginatedRequestModel
    {
        public int PageSize { get; set; } = 20;

        public int PageNumber { get; set; } = 1;
    }
}
