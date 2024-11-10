using Dapper;
using Yarnique.Common.Application.Data;
using Yarnique.Modules.Designs.Application.Configuration.Queries;
using Yarnique.Modules.Designs.Application.DesignCreation.GetAllDesignParts;

namespace Yarnique.Modules.Designs.Application.DesignCreation.GetDesign
{
    internal class GetDesignQueryHandler : IQueryHandler<GetDesignQuery, DesignDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetDesignQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<DesignDto> Handle(GetDesignQuery query, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            var lookup = new Dictionary<Guid, DesignDto>();
            const string sql = $"""
                       SELECT 
                           [d].[Id] 
                           , [d].[Name]
                           , [d].[Price]
                           , [d].[Published] AS [{nameof(DesignDto.IsPublished)}]
                           , [dps].[Id]
                           , [dp].[Name] AS DesignPartName
                           , [dps].[YarnAmount]
                                
                       FROM [designs].[Designs] AS [d]
                       LEFT JOIN [designs].[DesignPartSpecifications] AS dps ON dps.DesignId = d.Id
                       LEFT JOIN [designs].[DesignParts] AS dp ON dps.DesignPartId = dp.Id
                       WHERE d.Id = @Id
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
                new { Id = query.DesignId },
                splitOn: "Id,Id,Id"
            );

            return lookup.Values.FirstOrDefault();
        }
    }
}
