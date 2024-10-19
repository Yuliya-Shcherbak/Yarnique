using Yarnique.Modules.Designs.Application.Contracts;

namespace Yarnique.Modules.Designs.Application.DesignCreation.CreateDesignPart
{
    public class CreateDesignPartCommand : CommandBase
    {
        public CreateDesignPartCommand( string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
