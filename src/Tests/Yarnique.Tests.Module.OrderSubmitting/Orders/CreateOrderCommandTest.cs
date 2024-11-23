using Yarnique.Modules.OrderSubmitting.Application.Orders.CreateOrder;
using Yarnique.Test.Common.SqlCommands;

namespace Yarnique.Tests.Module.OrderSubmitting.Orders
{
    public class CreateOrderCommandTest : TestBase
    {
        [Fact]
        public async Task CreateOrderCommandHandler_ShouldReturnDesigns()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var designInfo = await AddDesign();
            await Users.AddUser(ConnectionString, userId);
            var command = new CreateOrderCommand(userId, designInfo.designId, GetDateOnly());

            // Act
            var orderId = await _ordersModule.ExecuteCommandAsync(command);

            // Assert
            Assert.NotEqual(orderId, Guid.Empty);
            CleanUp(designInfo.designPartIds.ToArray(), designInfo.designPartSpecificationIds.ToArray(), [designInfo.designId], [orderId], [userId]);
        }

        private DateOnly GetDateOnly()
        {
            var dateStr = DateTime.Now.AddDays(4).ToString("yyyy-MM-dd");
            return DateOnly.Parse(dateStr);
        }
    }
}
