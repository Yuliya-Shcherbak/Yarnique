using Yarnique.Common.Domain;

namespace Yarnique.Modules.UsersManagement.Domain.Users
{
    public class ApplicationRole : ValueObject
    {
        public static ApplicationRole Customer => new ApplicationRole(nameof(Customer));

        public static ApplicationRole Seller => new ApplicationRole(nameof(Seller));

        public static ApplicationRole Admin => new ApplicationRole(nameof(Admin));

        public string Value { get; }

        private ApplicationRole(string value)
        {
            this.Value = value;
        }
    }
}
