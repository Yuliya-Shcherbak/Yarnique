using Yarnique.Common.Application.Contracts;

namespace Yarnique.Modules.UsersManagement.Application.Contracts
{
    public interface IUsersManagementModule
    {
        Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

        Task ExecuteCommandAsync(ICommand command);

        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
    }
}
