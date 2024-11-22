using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Yarnique.Modules.Designs.Domain.Designs.Designs;
using Yarnique.Modules.Designs.Domain.Designs.DesignPartSpecifications;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;
using Yarnique.Modules.Designs.Domain.Users;

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
            builder.Property<UserId>("_sellerId").HasColumnName("SellerId");
            builder.Property<bool>("_published").HasColumnName("Published");

            builder.OwnsMany<DesignPartSpecification>("_parts", y =>
            {
                y.WithOwner().HasForeignKey("DesignId");
                y.ToTable("DesignPartSpecifications", "designs");
                y.HasKey(x => x.Id);
                y.Property<DesignId>("DesignId");
                y.Property<DesignPartId>("_designPartId").HasColumnName("DesignPartId");
                y.Property<int>("_yarnAmount").HasColumnName("YarnAmount");
                y.Property<int>("_executionOrder").HasColumnName("ExecutionOrder");
                y.Property<string>("_term").HasColumnName("Term");
            });
        }
    }
}
