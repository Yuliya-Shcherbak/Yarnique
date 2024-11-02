using Yarnique.Common.Domain;

namespace Yarnique.Modules.OrderSubmitting.Domain.Users
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}
