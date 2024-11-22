using FluentMigrator;
using FluentMigrator.SqlServer;

namespace Yarnique.Database.Migrations.Scripts
{
    [Migration(202410260003)]
    public class AddAuthentication : Migration
    {
        public override void Up()
        {
            Create.Schema("[users]").Authorization("dbo");

            Create.Table("[Users]").InSchema("[users]")
                .WithColumn("Id").AsGuid().PrimaryKey("PK_users_Users_Id")
                .WithColumn("Username").AsString()
                .WithColumn("FirstName").AsString()
                .WithColumn("LastName").AsString()
                .WithColumn("Email").AsString()
                .WithColumn("Password").AsString()
                .WithColumn("PasswordSalt").AsString()
                .WithColumn("IsActive").AsBoolean()
                .WithColumn("Role").AsString();

            Insert.IntoTable("[Users]").InSchema("[users]")
               .Row(new
               {
                   Id = Guid.NewGuid(),
                   UserName = "admin",
                   FirstName = "Yarnique",
                   LastName = "Admin",
                   Email = "admin@yarnique.com",
                   Password = "b441325a2f4fe7d905773283869327da0cfbcf25ce6c791c97166421ba8b8fe8",
                   PasswordSalt = "a7179a5690ed0da54896b55adbe77a63330bcf848a1726a3",
                   IsActive = true,
                   Role = "Admin"
               });
        }

        public override void Down()
        {
            Delete.Schema("[users]");
            Delete.Table("[Users]").InSchema("[users]");
        }
    }
}
