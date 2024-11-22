using FluentMigrator;
using FluentMigrator.SqlServer;
using Yarnique.Database.Migrations.Extensions;

namespace Yarnique.Database.Migrations.Scripts
{
    [Migration(202410200002)]
    public class AddBasicOrdersSchemaStructure : Migration
    {
        public override void Up()
        {
            Create.Schema("[orders]").Authorization("dbo");

            Create.Table("[DesignParts]").InSchema("[orders]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_orders_DesignParts_Id")
                .WithColumn("Name").AsString();

            Create.Table("[Designs]").InSchema("[orders]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_orders_Designs_Id")
                .WithColumn("Name").AsString()
                .WithColumn("Price").AsDouble()
                .WithColumn("Discontinued").AsBoolean();

            Create.Table("[DesignPartSpecifications]")
                .InSchema("[orders]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_orders_DesignPartSpecification_Id")
                .WithColumn("DesignId").AsGuid().ForeignKey("FK_orders_DesignPartSpecification_orders_Designs", "[orders]", "[Designs]", "[Id]")
                .WithColumn("DesignPartId").AsGuid().ForeignKey("FK_orders_DesignPartSpecification_orders_DesignParts", "[orders]", "[DesignParts]", "[Id]")
                .WithColumn("YarnAmount").AsInt32();

            Create.Table("[InboxMessages]")
                .InSchema("[orders]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_orders_InboxMessages_Id")
                .WithColumn("OccurredOn").AsDateTime()
                .WithColumn("Type").AsString()
                .WithColumn("Data").AsMaxString()
                .WithColumn("ProcessedDate").AsDateTime().Nullable();

            Create.Table("[InternalCommands]")
                .InSchema("[orders]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_orders_InternalCommands_Id")
                .WithColumn("EnqueueDate").AsDateTime()
                .WithColumn("Type").AsString()
                .WithColumn("Data").AsMaxString()
                .WithColumn("ProcessedDate").AsDateTime().Nullable()
                .WithColumn("Error").AsMaxString().Nullable();

            Create.Table("[OutboxMessages]")
                .InSchema("[orders]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_orders_OutboxMessages_Id")
                .WithColumn("OccurredOn").AsDateTime()
                .WithColumn("Type").AsString()
                .WithColumn("Data").AsMaxString()
                .WithColumn("ProcessedDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Schema("[orders]");
            Delete.Table("[DesignParts]").InSchema("[orders]");
            Delete.Table("[Designs]").InSchema("[orders]");
            Delete.Table("[DesignPartSpecifications]").InSchema("[orders]");
            Delete.Table("[InboxMessages]").InSchema("[orders]");
            Delete.Table("[InternalCommands]").InSchema("[orders]");
            Delete.Table("[OutboxMessages]").InSchema("[orders]");
        }
    }
}
