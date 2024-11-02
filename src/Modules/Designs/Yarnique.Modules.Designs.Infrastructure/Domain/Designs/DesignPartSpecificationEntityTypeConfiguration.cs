using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Yarnique.Modules.Designs.Domain.Designs.DesignPartSpecifications;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;

namespace Yarnique.Modules.Designs.Infrastructure.Domain.Designs
{
    internal class DesignPartSpecificationEntityTypeConfiguration : IEntityTypeConfiguration<DesignPartSpecification>
    {
        public void Configure(EntityTypeBuilder<DesignPartSpecification> builder)
        {
            builder.ToTable("DesignPartSpecifications", "designs");

            builder.HasKey(x => x.Id);

            builder.Property<int>("_yarnAmount").HasColumnName("YarnAmount");

            builder.HasOne<DesignPart>()
                .WithOne()
                .HasForeignKey(nameof(DesignPartSpecification), "_designPartId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<DesignPartId>("_designPartId").HasColumnName("DesignPartId");
            builder.Property<string>("_term").HasColumnName("Term");
        }
    }
}
