using System.Reflection;
using Yarnique.Modules.Designs.Application.Contracts;

namespace Yarnique.Modules.Designs.Infrastructure.Configuration
{
    internal class Assemblies
    {
        public static readonly Assembly Application = typeof(IDesignsModule).Assembly;
    }
}
