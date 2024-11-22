using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Modules.UsersManagement.Application.Authentication.PasswordManagement;
using Yarnique.Modules.UsersManagement.Domain.Users;

namespace Yarnique.Modules.UsersManagement.Application.Users.CreateUser
{
    internal class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IUsersManagementRepository _userRepository;

        public CreateUserCommandHandler(IUsersManagementRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var salt = PasswordHasher.CreateSalt();
            var hashedPassword = PasswordHasher.GetHash(command.Password, salt);

            var user = User.CreateCustomer(
                command.UserName,
                command.FirstName,
                command.LastName,
                command.Email,
                hashedPassword,
                salt,
                command.IsActive);

            await _userRepository.AddAsync(user);
            return user.Id.Value;
        }
    }
}
