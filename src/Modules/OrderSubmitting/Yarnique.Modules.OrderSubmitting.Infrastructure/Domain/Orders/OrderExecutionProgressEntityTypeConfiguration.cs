using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yarnique.Common.Domain.OrderStatuses;
using Yarnique.Modules.OrderSubmitting.Domain.Designs;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.OrderExecutions;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Domain.Orders
{
    internal class OrderExecutionProgressEntityTypeConfiguration : IEntityTypeConfiguration<OrderExecution>
    {
        public void Configure(EntityTypeBuilder<OrderExecution> builder)
        {
            builder.ToTable("OrderExecutionProgress", "orders");

            builder.HasKey(x => x.Id);

            builder.Property<OrderId>("_orderId").HasColumnName("OrderId");
            builder.Property<DesignPartSpecificationId>("_designPartSpecificationId").HasColumnName("DesignPartSpecificationId");
            builder.Property<DateTime>("_dueDate").HasColumnName("DueDate");
            builder.OwnsOne<ExecutionStatus>("_status", b =>
            {
                b.Property(p => p.Value).HasColumnName("Status");
            });
        }
    }
}
