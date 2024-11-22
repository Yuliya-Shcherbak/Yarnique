using Dapper;
using Yarnique.Common.Application.Data;
using Yarnique.Common.Application.Pagination;
using Yarnique.Common.Application.Configuration.Queries;

namespace Yarnique.Modules.Designs.Application.DesignCreation.GetAllDesignParts
{
    internal class GetAllDesignPartsQueryHandler : IQueryHandler<GetAllDesignPartsQuery, PaginatedResponse<DesignPartDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetAllDesignPartsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<PaginatedResponse<DesignPartDto>> Handle(GetAllDesignPartsQuery query, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = $"""
                       SELECT 
                           [dp].[Id] AS [{nameof(DesignPartDto.Id)}], 
                           [dp].[Name] AS [{nameof(DesignPartDto.Name)}]
                       FROM [designs].[DesignParts] AS [dp] 
                       ORDER BY [dp].[Name], [dp].[Id] ASC
                       OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
                       """;

            var result = await connection.QueryAsync<DesignPartDto>(sql, new { Offset = query.Offset, PageSize = query.PageSize });
            return new PaginatedResponse<DesignPartDto> { Items = result.AsList(), PageNumber = query.PageNumber, PageSize = query.PageSize };
        }
    }
}
