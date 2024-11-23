using Dapper;
using System.Data.SqlClient;
using Yarnique.Modules.Designs.Application.DesignCreation.CreateDesign;
using Yarnique.Modules.Designs.Application.DesignCreation.GetDesign;

namespace Yarnique.Test.Module.Designs.Designs
{
    public class CreateDesignCommandTest : TestBase
    {
        [Fact]
        public async Task CreateDesignCommandHandler_ShouldSaveDesign()
        {
            // Arrange
            var designPartIds = await SetUp();
            var userId = Guid.NewGuid();

            var designParts = new List<CreateDesignPartSpecificationCommand> {
                new CreateDesignPartSpecificationCommand(designPartIds[0], 50, 1, TimeSpan.FromDays(1).ToString()),
                new CreateDesignPartSpecificationCommand(designPartIds[1], 80, 2, TimeSpan.FromDays(2).ToString()),
            };

            // Act
            var designId = await _designsModule.ExecuteCommandAsync(
                new CreateDesignCommand("Creative Design", 123, userId, designParts));

            // Assert
            var design = await _designsModule.ExecuteQueryAsync(new GetDesignQuery(designId));

            Assert.NotNull(design);
            Assert.Equal("Creative Design", design.Name);
            Assert.Equal(123, design.Price);
            Assert.Equal(2, design.Parts.Count);
            CleanUp(designPartIds.ToArray(), design.Parts.Select(x => x.Id).ToArray(), [designId], [userId]);
        }

        private async Task<List<Guid>> SetUp()
        {
            var designPartIds = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                foreach (var id in designPartIds)
                    await sqlConnection.ExecuteScalarAsync("INSERT INTO [designs].[DesignParts] VALUES (@Id, @Name) ", new { Id = id, Name = $"DP-{Guid.NewGuid()}" });
            }

            return designPartIds;
        }
    }
}
