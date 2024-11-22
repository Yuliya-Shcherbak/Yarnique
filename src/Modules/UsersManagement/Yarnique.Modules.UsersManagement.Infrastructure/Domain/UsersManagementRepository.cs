using Microsoft.EntityFrameworkCore;
using Yarnique.Modules.UsersManagement.Domain.Users;

namespace Yarnique.Modules.UsersManagement.Infrastructure.Domain
{
    public class UsersManagementRepository : IUsersManagementRepository
    {
        private readonly UsersManagementContext _usersManagementContext;

        public UsersManagementRepository(UsersManagementContext usersManagementContext)
        {
            _usersManagementContext = usersManagementContext;
        }

        public async Task AddAsync(User design)
        {
            await _usersManagementContext.Users.AddAsync(design);
        }

        public async Task<User> GetByIdAsync(UserId id)
        {
            return await _usersManagementContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
