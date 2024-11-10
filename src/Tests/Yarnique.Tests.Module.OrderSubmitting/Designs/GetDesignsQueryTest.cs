using Dapper;
using System.Data.SqlClient;
using Yarnique.Modules.OrderSubmitting.Application.Designs.GetDesigns;

namespace Yarnique.Tests.Module.OrderSubmitting.Designs
{
    public class GetDesignsQueryTest : TestBase
    {
        [Fact]
        public async Task GetDesignsQueryHandler_ShouldReturnDesigns()
        {
            // Arrange
            var designId = Guid.NewGuid();
            await AddDesign(designId);
            var query = new GetDesignsQuery();

            // Act
            var designs = await _ordersModule.ExecuteQueryAsync(query);

            // Assert
            Assert.NotNull(designs);
            var design = designs.FirstOrDefault(x => x.Id == designId);
            Assert.NotNull(design);
            Assert.Equal("Tiny Rabbit", design.Name);
            Assert.Equal(2, design.Parts.Count);
        }
    }
}
