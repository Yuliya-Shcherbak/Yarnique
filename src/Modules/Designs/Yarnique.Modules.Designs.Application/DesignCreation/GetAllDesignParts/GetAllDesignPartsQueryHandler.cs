using Dapper;
using Yarnique.Common.Application.Data;
using Yarnique.Modules.Designs.Application.Configuration.Queries;

namespace Yarnique.Modules.Designs.Application.DesignCreation.GetAllDesignParts
{
    internal class GetAllDesignPartsQueryHandler : IQueryHandler<GetAllDesignPartsQuery, List<DesignPartDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetAllDesignPartsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<DesignPartDto>> Handle(GetAllDesignPartsQuery query, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = $"""
                       SELECT 
                           [dp].[Id] AS [{nameof(DesignPartDto.Id)}], 
                           [dp].[Name] AS [{nameof(DesignPartDto.Name)}]
                       FROM [designs].[DesignParts] AS [dp] 
                       ORDER BY [dp].[Name] ASC
                       """;
            var result = await connection.QueryAsync<DesignPartDto>(sql);
            return result.AsList();
        }
    }
}
