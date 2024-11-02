using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yarnique.Common.Application.Outbox;
using Yarnique.Modules.UsersManagement.Domain.Users;
using Yarnique.Modules.UsersManagement.Infrastructure.Domain.Users;
using Yarnique.Modules.UsersManagement.Infrastructure.Outbox;

namespace Yarnique.Modules.UsersManagement.Infrastructure
{
    public class UsersManagementContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<User> Users { get; set; }

        internal DbSet<OutboxMessage> OutboxMessages { get; set; }

        public UsersManagementContext(DbContextOptions options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        }
    }
}
