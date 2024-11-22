using System.Reflection;
using Yarnique.Modules.OrderSubmitting.Application.Contracts;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration
{
    internal class Assemblies
    {
        public static readonly Assembly Application = typeof(IOrderSubmittingModule).Assembly;
    }
}
