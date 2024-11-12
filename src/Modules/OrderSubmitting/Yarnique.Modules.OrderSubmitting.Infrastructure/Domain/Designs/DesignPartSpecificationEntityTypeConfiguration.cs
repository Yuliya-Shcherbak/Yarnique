using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yarnique.Modules.OrderSubmitting.Domain.Designs;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Domain.Designs
{
    internal class DesignPartSpecificationEntityTypeConfiguration : IEntityTypeConfiguration<DesignPartSpecification>
    {
        public void Configure(EntityTypeBuilder<DesignPartSpecification> builder)
        {
            builder.ToTable("DesignPartSpecifications", "orders");

            builder.HasKey(x => x.Id);

            builder.Property<DesignId>("_designId").HasColumnName("DesignId");

            builder.Property<int>("_executionOrder").HasColumnName("ExecutionOrder");

            builder.Property<string>("_term").HasColumnName("Term");
        }
    }
}
