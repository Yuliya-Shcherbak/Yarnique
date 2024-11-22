using Dapper;
using System.Data.SqlClient;

namespace Yarnique.Test.Common.SqlCommands
{
    public static class Users
    {
        public static async Task AddUser(string connectionString, Guid userId, string role = "Customer")
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [users].[Users] VALUES (@Id, @UserName, @FirstName, @LastName, @Email, @Password, @PasswordSalt, @IsActive, @Role)",
                new
                {
                    Id = userId,
                    UserName = "customer1",
                    FirstName = "Customer1",
                    LastName = "Customer1",
                    Email = "customer1@yarnique.com",
                    Password = "b441325a2f4fe7d905773283869327da0cfbcf25ce6c791c97166421ba8b8fe8",
                    PasswordSalt = "a7179a5690ed0da54896b55adbe77a63330bcf848a1726a3",
                    IsActive = true,
                    Role = role
                });
            }
        }
    }
}
