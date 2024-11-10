using FluentMigrator;
using FluentMigrator.SqlServer;
using Yarnique.Database.Migrations.Extensions;

namespace Yarnique.Database.Migrations.Scripts
{
    [Migration(202410200001)]
    public class InitialStructure : Migration
    {
        public override void Up()
        {
            Create.Schema("[designs]").Authorization("dbo");

            Create.Table("[DesignParts]").InSchema("[designs]")
                .WithColumn("[Id]").AsGuid().PrimaryKey("[PK_consumables_DesignParts_Id]")
                .WithColumn("[Name]").AsString();

            Create.Table("[Designs]").InSchema("[designs]")
                .WithColumn("[Id]").AsGuid().PrimaryKey("[PK_designs_Designs_Id]")
                .WithColumn("[Name]").AsString()
                .WithColumn("[Price]").AsDouble()
                .WithColumn("[Published]").AsBoolean();

            Create.Table("[DesignPartSpecifications]").InSchema("[designs]")
                .WithColumn("[Id]").AsGuid().PrimaryKey("[PK_designs_DesignPartSpecification_Id]")
                .WithColumn("DesignId").AsGuid().ForeignKey("FK_designs_DesignPartSpecification_designs_Designs", "[designs]", "[Designs]", "[Id]")
                .WithColumn("DesignPartId").AsGuid().ForeignKey("FK_designs_DesignPartSpecification_designs_DesignParts", "[designs]", "[DesignParts]", "[Id]")
                .WithColumn("YarnAmount").AsInt32();

            Create.Table("[InboxMessages]").InSchema("[designs]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_designs_InboxMessages_Id")
                .WithColumn("OccurredOn").AsDateTime()
                .WithColumn("Type").AsString()
                .WithColumn("Data").AsMaxString()
                .WithColumn("ProcessedDate").AsDateTime().Nullable();

            Create.Table("[InternalCommands]").InSchema("[designs]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_designs_InternalCommands_Id")
                .WithColumn("EnqueueDate").AsDateTime()
                .WithColumn("Type").AsString()
                .WithColumn("Data").AsMaxString()
                .WithColumn("ProcessedDate").AsDateTime().Nullable()
                .WithColumn("Error").AsMaxString().Nullable();

            Create.Table("[OutboxMessages]").InSchema("[designs]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_designs_OutboxMessages_Id")
                .WithColumn("OccurredOn").AsDateTime()
                .WithColumn("Type").AsString()
                .WithColumn("Data").AsMaxString()
                .WithColumn("ProcessedDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Schema("[designs]");
            Delete.Table("[DesignParts]").InSchema("[designs]");
            Delete.Table("[Designs]").InSchema("[designs]");
            Delete.Table("[DesignPartSpecifications]").InSchema("[designs]");
            Delete.Table("[InboxMessages]").InSchema("[designs]");
            Delete.Table("[InternalCommands]").InSchema("[designs]");
            Delete.Table("[OutboxMessages]").InSchema("[designs]");
        }
    }
}
