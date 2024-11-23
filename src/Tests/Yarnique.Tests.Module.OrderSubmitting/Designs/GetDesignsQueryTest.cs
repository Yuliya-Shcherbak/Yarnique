using Yarnique.Modules.OrderSubmitting.Application.Designs.GetDesigns;
using Yarnique.Modules.OrderSubmitting.Domain.Orders.Orders;

namespace Yarnique.Tests.Module.OrderSubmitting.Designs
{
    public class GetDesignsQueryTest : TestBase
    {
        [Fact]
        public async Task GetDesignsQueryHandler_ShouldReturnDesigns()
        {
            // Arrange
            var designInfo = await AddDesign();
            var query = new GetDesignsQuery();

            // Act
            var designs = await _ordersModule.ExecuteQueryAsync(query);

            // Assert
            Assert.NotNull(designs);
            var design = designs.Items.FirstOrDefault(x => x.Id == designInfo.designId);
            Assert.NotNull(design);
            Assert.Equal("Tiny Rabbit", design.Name);
            Assert.Equal(2, design.Parts.Count);
            CleanUp(designInfo.designPartIds.ToArray(), designInfo.designPartSpecificationIds.ToArray(), [designInfo.designId], Array.Empty<Guid>(), Array.Empty<Guid>());
        }
    }
}
