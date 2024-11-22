namespace Yarnique.API.Modules.Designs.Designs
{
    public class CreateDesignRequest
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public List<DesignPartSpecificationRequest> Parts { get; set; }
    }
}
