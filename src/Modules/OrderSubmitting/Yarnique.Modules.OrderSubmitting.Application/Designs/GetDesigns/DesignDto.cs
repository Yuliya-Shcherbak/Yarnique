namespace Yarnique.Modules.OrderSubmitting.Application.Designs.GetDesigns
{
    public class DesignDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public bool IsPublished { get; set; }

        public List<DesignPartsSpecificationDto> Parts { get; set; }
    }
}
