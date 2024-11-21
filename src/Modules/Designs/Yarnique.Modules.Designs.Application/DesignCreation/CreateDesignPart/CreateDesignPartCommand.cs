using Yarnique.Common.Application.Configuration.Attributes;
using Yarnique.Common.Application.Contracts;

namespace Yarnique.Modules.Designs.Application.DesignCreation.CreateDesignPart
{
    [CacheableEntity("DesignPart")]
    public class CreateDesignPartCommand : CommandBase
    {
        public CreateDesignPartCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
