namespace Yarnique.API.Modules.Designs.Designs
{
    public class DesignPartSpecificationRequest
    {
        public Guid DesignPartId { get; set; }
        public int YarnAmount { get; set; }
        public string Term { get; set; }
    }
}
