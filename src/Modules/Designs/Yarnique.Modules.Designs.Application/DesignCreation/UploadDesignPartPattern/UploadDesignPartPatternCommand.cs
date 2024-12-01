using Microsoft.AspNetCore.Http;
using Yarnique.Common.Application.Contracts;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;

namespace Yarnique.Modules.Designs.Application.DesignCreation.UploadDesignPartPattern
{
    public class UploadDesignPartPatternCommand : CommandBase
    {
        public UploadDesignPartPatternCommand(Guid designPartId, IFormFile file)
        {
            DesignPartId = new DesignPartId(designPartId);
            File = file;
        }

        public DesignPartId DesignPartId { get; }

        public IFormFile File { get; }
    }
}
