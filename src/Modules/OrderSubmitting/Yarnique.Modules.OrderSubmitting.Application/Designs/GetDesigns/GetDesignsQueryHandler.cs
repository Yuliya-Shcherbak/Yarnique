using Dapper;
using Yarnique.Common.Application.Data;
using Yarnique.Common.Application.Configuration.Queries;
using Yarnique.Common.Application.Pagination;

namespace Yarnique.Modules.OrderSubmitting.Application.Designs.GetDesigns
{
    internal class GetDesignsQueryHandler : IQueryHandler<GetDesignsQuery, PaginatedResponse<DesignDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetDesignsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<PaginatedResponse<DesignDto>> Handle(GetDesignsQuery query, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            var lookup = new Dictionary<Guid, DesignDto>();
            const string sql = $"""
                       SELECT 
                           [d].[Id] 
                           , [d].[Name]
                           , [d].[Price]
                           , [dps].[Id]
                           , [dp].[Name] AS DesignPartName
                           , [dps].[YarnAmount]
                                
                       FROM [orders].[Designs] AS [d]
                       LEFT JOIN [orders].[DesignPartSpecifications] AS dps ON dps.DesignId = d.Id
                       LEFT JOIN [orders].[DesignParts] AS dp ON dps.DesignPartId = dp.Id
                       WHERE [d].[Discontinued] = 0
                       ORDER BY [d].[Name], [d].[Id] ASC
                       OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
                       """;
            await connection.QueryAsync<DesignDto, DesignPartsSpecificationDto, DesignDto>(
                sql,
                (d, dps) =>
                {
                    DesignDto design;

                    if (!lookup.TryGetValue(d.Id, out design))
                        lookup.Add(d.Id, design = d);

                    design.Parts ??= new();
                    if (dps != null)
                        design.Parts.Add(dps);

                    return null;
                },
                param: new { Offset = query.Offset, PageSize = query.PageSize },
                splitOn: "Id,Id,Id"
            );

            return new PaginatedResponse<DesignDto> { Items = lookup.Values.ToList(), PageNumber = query.PageNumber, PageSize = query.PageSize };
        }
    }
}
