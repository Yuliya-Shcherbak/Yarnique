using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Yarnique.Modules.Designs.Domain.Designs.Designs;
using Yarnique.Modules.Designs.Domain.Designs.DesignPartSpecifications;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;

namespace Yarnique.Modules.Designs.Infrastructure.Domain.Designs
{
    internal class DesignEntityTypeConfiguration : IEntityTypeConfiguration<Design>
    {
        public void Configure(EntityTypeBuilder<Design> builder)
        {
            builder.ToTable("Designs", "designs");

            builder.HasKey(x => x.Id);

            builder.Property<string>("_name").HasColumnName("Name");
            builder.Property<double>("_price").HasColumnName("Price");
            builder.Property<bool>("_published").HasColumnName("Published");

            builder.OwnsMany<DesignPartSpecification>("_parts", y =>
            {
                y.WithOwner().HasForeignKey("DesignId");
                y.ToTable("DesignPartSpecifications", "designs");
                y.HasKey(x => x.Id);
                y.Property<DesignId>("DesignId");
                y.Property<DesignPartId>("_designPartId").HasColumnName("DesignPartId");
                y.Property<int>("_yarnAmount").HasColumnName("YarnAmount");
            });
        }
    }
}
