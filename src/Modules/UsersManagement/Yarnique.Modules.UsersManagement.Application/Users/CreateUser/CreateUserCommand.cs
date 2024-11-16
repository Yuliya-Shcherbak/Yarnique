using Yarnique.Common.Application.Contracts;

namespace Yarnique.Modules.UsersManagement.Application.Users.CreateUser
{
    public class CreateUserCommand : CommandBase<Guid>
    {
        public CreateUserCommand(string userName, string firstName, string lastName, string email, string password, bool isActive)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            IsActive = isActive;
        }

        public string UserName { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string Password { get; }

        public bool IsActive { get; }
    }
}
