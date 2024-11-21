namespace Yarnique.Common.Application.Pagination
{
    public interface IPaginatedRequest
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public int Offset { get; }
    }
}
