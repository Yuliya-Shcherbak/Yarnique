using FluentMigrator;

namespace Yarnique.Database.Migrations.Scripts
{
    [Migration(202411100007)]
    public class AlterDesignPartTablesWithBlobName : Migration
    {
        public override void Up()
        {
            Alter.Table("DesignParts").InSchema("[designs]")
                .AddColumn("BlobName").AsString().Nullable();

            Alter.Table("DesignParts").InSchema("[orders]")
                .AddColumn("BlobName").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("BlobName").FromTable("DesignParts").InSchema("[designs]");
            Delete.Column("BlobName").FromTable("DesignParts").InSchema("[orders]");
        }
    }
}
