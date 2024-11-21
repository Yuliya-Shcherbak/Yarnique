using Yarnique.Common.Application.Pagination;

namespace Yarnique.Common.Application.Contracts
{
    public abstract class QueryBaseWithPaging<TResult> : QueryBase<TResult>, IPaginatedRequest
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public int Offset { get; }

        protected QueryBaseWithPaging(int pageNumber = 1, int pageSize = 5)
            : base()
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Offset = (PageNumber - 1) * PageSize;
        }

        public void Deconstruct(out int pageNumber, out int pageSize)
        {
            pageNumber = PageNumber;
            pageSize = PageSize;
        }
    }
}
