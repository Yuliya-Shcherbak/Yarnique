using Autofac;
using MediatR;
using Yarnique.Common.Application.Contracts;
using Yarnique.Modules.UsersManagement.Application.Contracts;
using Yarnique.Modules.UsersManagement.Infrastructure.Configuration;
using Yarnique.Modules.UsersManagement.Infrastructure.Configuration.Processing;

namespace Yarnique.Modules.UsersManagement.Infrastructure
{
    public class UsersManagementModule : IUsersManagementModule
    {
        public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
        {
            return await CommandsExecutor.Execute(command);
        }

        public async Task ExecuteCommandAsync(ICommand command)
        {
            await CommandsExecutor.Execute(command);
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            using (var scope = UsersManagementCompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();

                return await mediator.Send(query);
            }
        }
    }
}
