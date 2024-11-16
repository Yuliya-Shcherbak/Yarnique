using Newtonsoft.Json;
using Yarnique.Common.Application.Configuration.Commands;

namespace Yarnique.Modules.OrderSubmitting.Application.Designs.PublishDesign
{
    public class CopyPublishedDesignCommand : InternalCommandBase
    {
        [JsonConstructor]
        public CopyPublishedDesignCommand(Guid id, Guid designId)
            : base(id)
        {
            DesignId = designId;
        }

        internal Guid DesignId { get; }
    }
}
