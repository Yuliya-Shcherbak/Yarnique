using Dapper;
using System.Data.SqlClient;
using Yarnique.Modules.OrderSubmitting.Application.Orders.CreateOrder;

namespace Yarnique.Tests.Module.OrderSubmitting.Orders
{
    public class CreateOrderCommandTest : TestBase
    {
        [Fact]
        public async Task GetDesignsQueryHandler_ShouldReturnDesigns()
        {
            // Arrange
            var designId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            await AddDesign(designId);
            await AddUser(userId);
            var command = new CreateOrderCommand(userId, designId, GetDateOnly());

            // Act
            var order = await _ordersModule.ExecuteCommandAsync(command);

            // Assert
            Assert.NotEqual(order, Guid.Empty);
        }

        private DateOnly GetDateOnly()
        {
           var dateStr = DateTime.Now.AddDays(4).ToString("yyyy-MM-dd");
            return DateOnly.Parse(dateStr);
        }

        private async Task AddUser(Guid userId)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [users].[Users] VALUES (@Id, @UserName, @FirstName, @LastName, @Email, @Password, @PasswordSalt, @IsActive, @Role) ",
                new { 
                    Id = userId,
                    UserName = "customer1",
                    FirstName = "Customer1",
                    LastName = "Customer1",
                    Email = "customer1@yarnique.com",
                    Password = "b441325a2f4fe7d905773283869327da0cfbcf25ce6c791c97166421ba8b8fe8",
                    PasswordSalt = "a7179a5690ed0da54896b55adbe77a63330bcf848a1726a3",
                    IsActive = true,
                    Role = "Customer"
                });
            }
        }
    }
}
