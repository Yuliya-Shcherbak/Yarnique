using System.Reflection;
using Yarnique.Modules.UsersManagement.Application.Contracts;

namespace Yarnique.Modules.UsersManagement.Infrastructure.Configuration
{
    internal class Assemblies
    {
        public static readonly Assembly Application = typeof(IUsersManagementModule).Assembly;
    }
}
