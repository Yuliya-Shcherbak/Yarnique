using Yarnique.Common.Domain;
using Yarnique.Modules.UsersManagement.Domain.Users.Events;

namespace Yarnique.Modules.UsersManagement.Domain.Users
{
    public class User : Entity
    {
        public UserId Id { get; private set; }

        private string _userName;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private string _passwordSalt;
        private bool _isActive;
        private ApplicationRole _role;

        private User()
        {
        }

        public static User CreateAdmin(string userName, string firstName, string lastName, string email, string password, string passwordSalt)
        {
            return new User(Guid.NewGuid(), userName, firstName, lastName, email, password, passwordSalt, true, ApplicationRole.Admin);
        }

        public static User CreateCustomer(string userName, string firstName, string lastName, string email, string password, string passwordSalt, bool isActive)
        {
            return new User(Guid.NewGuid(), userName, firstName, lastName, email, password, passwordSalt, isActive, ApplicationRole.Customer);
        }

        private User(Guid id, string userName, string firstName, string lastName, string email, string password, string passwordSalt, bool isActive, ApplicationRole role)
        {
            Id = new UserId(id);
            _userName = userName;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _password = password;
            _passwordSalt = passwordSalt;
            _isActive = isActive;
            _role = role;

            AddDomainEvent(new UserCreatedDomainEvent(Id));
        }
    }
}
