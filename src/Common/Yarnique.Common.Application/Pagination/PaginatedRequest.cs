namespace Yarnique.Common.Application.Pagination
{
    public class PaginatedRequest : IPaginatedRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int Offset => (PageNumber - 1) * PageSize;
    }
}
