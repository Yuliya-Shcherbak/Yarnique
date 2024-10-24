using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yarnique.Common.Application.Outbox;
using Yarnique.Common.Infrastructure.InternalCommands;
using Yarnique.Modules.OrderSubmitting.Infrastructure.InternalCommands;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Outbox;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure
{
    public class OrderSubmittingContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;


        public DbSet<InternalCommand> InternalCommands { get; set; }
        internal DbSet<OutboxMessage> OutboxMessages { get; set; }

        public OrderSubmittingContext(DbContextOptions options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
        }
    }
}
