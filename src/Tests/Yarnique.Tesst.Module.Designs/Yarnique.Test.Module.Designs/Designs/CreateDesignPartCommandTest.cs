using Yarnique.Modules.Designs.Application.DesignCreation.CreateDesignPart;
using Yarnique.Modules.Designs.Application.DesignCreation.GetAllDesignParts;

namespace Yarnique.Test.Module.Designs.DesignCreation
{
    public class CreateDesignPartCommandTest : TestBase
    {
        [Fact]
        public async Task CreateDesignPartCommandHandler_ShouldSaveDesignPart()
        {
            // Arrange
            var designPartName = "Pretty Paw";
            var command = new CreateDesignPartCommand(designPartName);

            // Act
            await _designsModule.ExecuteCommandAsync(command);

            // Assert
            var designParts = await _designsModule.ExecuteQueryAsync(new GetAllDesignPartsQuery(1, 5));
            var designPart = designParts.Items.FirstOrDefault(x => x.Name == designPartName);
            Assert.NotNull(designPart);
            Assert.Equal(designPartName, designPart.Name);
        }
    }
}
