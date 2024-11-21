using FluentMigrator;

namespace Yarnique.Database.Migrations.Scripts
{
    [Migration(202411100006)]
    public class AlterDesignSpecificationAndOrdersTables : Migration
    {
        public override void Up()
        {
            Alter.Table("DesignPartSpecifications").InSchema("[designs]")
                .AddColumn("ExecutionOrder").AsInt32().WithDefaultValue(1);

            Alter.Table("Designs").InSchema("[designs]")
                .AddColumn("SellerId").AsGuid().WithDefaultValue(Guid.Empty);

            Alter.Table("DesignPartSpecifications").InSchema("[orders]")
                .AddColumn("ExecutionOrder").AsInt32().WithDefaultValue(1);

            Alter.Table("Designs").InSchema("[orders]")
                .AddColumn("SellerId").AsGuid().WithDefaultValue(Guid.Empty);

            Alter.Table("Orders").InSchema("[orders]")
                .AddColumn("AcceptedDate").AsDateTime().Nullable();

            Create.Table("OrderExecutionProgress").InSchema("[orders]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_orders_ExecutionProgress_Id")
                .WithColumn("OrderId").AsGuid().ForeignKey("FK_orders_OrderExecutionProgress_orders_Orders", "[orders]", "[Orders]", "[Id]")
                .WithColumn("DesignPartSpecificationId").AsGuid().ForeignKey("FK_orders_OrderExecutionProgress_orders_DesignPartSpecifications", "[orders]", "[DesignPartSpecifications]", "[Id]")
                .WithColumn("DueDate").AsDateTime()
                .WithColumn("Status").AsString();
        }

        public override void Down()
        {
            Delete.Column("ExecutionOrder").FromTable("DesignPartSpecifications").InSchema("[designs]");
            Delete.Column("SellerId").FromTable("Designs").InSchema("[designs]");
            Delete.Column("ExecutionOrder").FromTable("DesignPartSpecifications").InSchema("[orders]");
            Delete.Column("SellerId").FromTable("Designs").InSchema("[orders]");
            Delete.Column("AcceptedDate").FromTable("Orders").InSchema("[orders]");
            Delete.Table("OrderExecutionProgress").InSchema("[orders]");
        }
    }
}
