using Yarnique.Common.Domain;

namespace Yarnique.Modules.OrderSubmitting.Domain.Designs
{
    public class DesignPartSpecification : Entity
    {
        public DesignPartSpecificationId Id { get; private set; }

        private DesignId _designId;
        private int _executionOrder;
        private string _term;

        public DesignId DesignId { get; }
        public int ExecutionOrder { get; }
        public string Term { get; }

        DesignPartSpecification(DesignId _designId, int _executionOrder, string _term)
        {
            DesignId = _designId;
            ExecutionOrder = _executionOrder;
            Term = _term;
        }
    }
}
