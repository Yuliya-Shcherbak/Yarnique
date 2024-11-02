using Yarnique.Common.Domain;

namespace Yarnique.Modules.UsersManagement.Domain.Users
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}
