using Dapper;
using Yarnique.Common.Application.Data;
using Yarnique.Modules.OrderSubmitting.Application.Configuration.Queries;

namespace Yarnique.Modules.OrderSubmitting.Application.Designs.GetDesigns
{
    internal class GetDesignsQueryHandler : IQueryHandler<GetDesignsQuery, List<DesignDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetDesignsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<DesignDto>> Handle(GetDesignsQuery query, CancellationToken cancellationToken)
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
                splitOn: "Id,Id,Id"
            );

            return lookup.Values.ToList();
        }
    }
}
