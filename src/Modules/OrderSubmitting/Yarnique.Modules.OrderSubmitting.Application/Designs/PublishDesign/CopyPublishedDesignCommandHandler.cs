using Dapper;
using Yarnique.Common.Application.Data;
using Yarnique.Modules.OrderSubmitting.Application.Configuration.Commands;

namespace Yarnique.Modules.OrderSubmitting.Application.Designs.PublishDesign
{
    internal class CopyPublishedDesignCommandHandler : ICommandHandler<CopyPublishedDesignCommand>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CopyPublishedDesignCommandHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task Handle(CopyPublishedDesignCommand command, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            await connection.ExecuteAsync("exec [orders].[PublishDesign] @id", new { id = command.DesignId });
        }
    }
}
