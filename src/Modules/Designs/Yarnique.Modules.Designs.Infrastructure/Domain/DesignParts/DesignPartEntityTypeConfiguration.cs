using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;

namespace Yarnique.Modules.Designs.Infrastructure.Domain.DesignParts
{
    internal class DesignPartEntityTypeConfiguration : IEntityTypeConfiguration<DesignPart>
    {
        public void Configure(EntityTypeBuilder<DesignPart> builder)
        {
            builder.ToTable("DesignParts", "designs");

            builder.HasKey(x => x.Id);

            builder.Property<string>("_name").HasColumnName("Name");
        }
    }
}
