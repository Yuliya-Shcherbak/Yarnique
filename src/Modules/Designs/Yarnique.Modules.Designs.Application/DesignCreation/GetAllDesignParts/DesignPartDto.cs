using Yarnique.Common.Application.Configuration.Attributes;

namespace Yarnique.Modules.Designs.Application.DesignCreation.GetAllDesignParts
{
    [CacheableEntity("DesignPart")]
    public class DesignPartDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
