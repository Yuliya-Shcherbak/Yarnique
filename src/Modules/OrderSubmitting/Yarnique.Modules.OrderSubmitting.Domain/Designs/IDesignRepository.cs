namespace Yarnique.Modules.OrderSubmitting.Domain.Designs
{
    public interface IDesignRepository
    {
        Task<List<DesignPartSpecification>> GetDesignPartSpecificationsByDesignIdAsync(DesignId designId);
    }
}
