using Yarnique.Common.Domain;

namespace Yarnique.Modules.Designs.Domain.Users
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}
