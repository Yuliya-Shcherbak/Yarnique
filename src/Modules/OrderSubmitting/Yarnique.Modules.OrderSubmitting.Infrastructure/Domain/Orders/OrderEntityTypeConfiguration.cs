using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Yarnique.Modules.OrderSubmitting.Domain.Designs;
using Yarnique.Modules.OrderSubmitting.Domain.Users;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;
using Yarnique.Common.Domain.OrderStatuses;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Domain.Orders
{
    internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", "orders");

            builder.HasKey(x => x.Id);

            builder.Property<DesignId>("_designId").HasColumnName("DesignId");
            builder.Ignore("DesignId");
            builder.Property<UserId>("_userId").HasColumnName("UserId");
            builder.Property<bool>("_isPaid").HasColumnName("IsPaid");
            builder.Property<string>("_transactionId").HasColumnName("TransactionId");
            builder.Property<string>("_transactionError").HasColumnName("TransactionError");
            builder.OwnsOne<OrderStatus>("_status", b =>
            {
                b.Property(p => p.Value).HasColumnName("Status");
            });
            builder.Property<DateOnly>("_executionDate").HasColumnName("ExecutionDate");
            builder.Property<DateTime?>("_acceptedDate").HasColumnName("AcceptedDate");
        }
    }
}
