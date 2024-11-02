using Yarnique.Modules.UsersManagement.Domain.Identity;

namespace Yarnique.API.Configuration
{
    public class YarniqueConfig
    {
        public ConnectionStringsConfig ConnectionStrings { get; set; }

        public IdentityConfig Identity { get; set; }
    }

    public class ConnectionStringsConfig
    {
        public string YarniqueConnectionString { get; set; }
    }
}
