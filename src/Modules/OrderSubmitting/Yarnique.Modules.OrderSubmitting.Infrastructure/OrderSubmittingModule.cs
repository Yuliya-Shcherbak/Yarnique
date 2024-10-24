using Autofac;
using MediatR;
using Yarnique.Modules.OrderSubmitting.Application.Contracts;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Processing;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure
{
    public class OrderSubmittingModule : IOrderSubmittingModule
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
            using (var scope = OrderSubmittingCompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();

                return await mediator.Send(query);
            }
        }
    }
}
