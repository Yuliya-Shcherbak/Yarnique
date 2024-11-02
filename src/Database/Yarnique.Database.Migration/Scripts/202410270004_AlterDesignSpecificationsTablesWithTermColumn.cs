using FluentMigrator;

namespace Yarnique.Database.Migrations.Scripts
{
    [Migration(202410270004)]
    public class AlterDesignSpecificationsTablesWithTermColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("DesignPartSpecifications").InSchema("[designs]")
                .AddColumn("Term").AsString().WithDefaultValue("1.00:00:00");

            Alter.Table("DesignPartSpecifications").InSchema("[orders]")
                .AddColumn("Term").AsString().WithDefaultValue("1.00:00:00");
        }

        public override void Down()
        {
            Delete.Column("Term").FromTable("DesignPartSpecifications").InSchema("[designs]");
            Delete.Column("Term").FromTable("DesignPartSpecifications").InSchema("[orders]");
        }
    }
}
