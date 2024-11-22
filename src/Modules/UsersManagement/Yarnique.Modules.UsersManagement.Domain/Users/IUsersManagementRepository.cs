namespace Yarnique.Modules.UsersManagement.Domain.Users
{
    public interface IUsersManagementRepository
    {
        Task AddAsync(User user);
    }
}
