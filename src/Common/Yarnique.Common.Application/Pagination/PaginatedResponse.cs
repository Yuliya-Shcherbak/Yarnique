namespace Yarnique.Common.Application.Pagination
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
