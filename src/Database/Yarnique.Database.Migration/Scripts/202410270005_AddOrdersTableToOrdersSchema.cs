using FluentMigrator;

namespace Yarnique.Database.Migrations.Scripts
{
    [Migration(202410270005)]
    public class AddOrdersTableToOrdersSchema : Migration
    {
        public override void Up()
        {
            Create.Table("[Orders]").InSchema("[orders]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_orders_Orders_Id")
                .WithColumn("DesignId").AsGuid().ForeignKey("FK_orders_Orders_orders_Designs", "[orders]", "[Designs]", "[Id]")
                .WithColumn("UserId").AsGuid().ForeignKey("FK_orders_Orders_users_Users", "[users]", "[Users]", "[Id]")
                .WithColumn("IsPaid").AsBoolean()
                .WithColumn("TransactionId").AsString().Nullable()
                .WithColumn("TransactionError").AsString().Nullable()
                .WithColumn("Status").AsString()
                .WithColumn("ExecutionDate").AsDate();
        }

        public override void Down()
        {
            Delete.Table("[Orders]").InSchema("[orders]");
        }
    }
}
