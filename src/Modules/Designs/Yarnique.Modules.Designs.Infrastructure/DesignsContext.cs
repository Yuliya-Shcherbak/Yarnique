using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yarnique.Common.Application.Outbox;
using Yarnique.Common.Infrastructure.InternalCommands;
using Yarnique.Modules.Designs.Domain.Designs.Designs;
using Yarnique.Modules.Designs.Domain.Designs.DesignParts;
using Yarnique.Modules.Designs.Domain.Designs.DesignPartSpecifications;
using Yarnique.Modules.Designs.Infrastructure.Domain.DesignParts;
using Yarnique.Modules.Designs.Infrastructure.Domain.Designs;
using Yarnique.Modules.Designs.Infrastructure.InternalCommands;
using Yarnique.Modules.Designs.Infrastructure.Outbox;

namespace Yarnique.Modules.Designs.Infrastructure
{
    public class DesignsContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<DesignPart> DesignParts { get; set; }
        public DbSet<Design> Designs { get; set; }
        public DbSet<DesignPartSpecification> DesignPartSpecifications { get; set; }


        public DbSet<InternalCommand> InternalCommands { get; set; }
        internal DbSet<OutboxMessage> OutboxMessages { get; set; }

        public DesignsContext(DbContextOptions options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DesignEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DesignPartEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
        }
    }
}
