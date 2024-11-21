using Yarnique.Modules.OrderSubmitting.Application.Orders.CreateOrder;
using Yarnique.Test.Common.SqlCommands;

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
            await Users.AddUser(ConnectionString, userId);
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
    }
}
