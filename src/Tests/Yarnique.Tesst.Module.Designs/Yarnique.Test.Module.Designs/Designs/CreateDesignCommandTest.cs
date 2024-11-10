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
            var designPartIdFirst = Guid.NewGuid();
            var designPartIdSecond = Guid.NewGuid();
            await SetUp(designPartIdFirst, designPartIdSecond);

            var designParts = new List<CreateDesignPartSpecificationCommand> {
                new CreateDesignPartSpecificationCommand(designPartIdFirst, 50, TimeSpan.FromDays(1).ToString()),
                new CreateDesignPartSpecificationCommand(designPartIdSecond, 80, TimeSpan.FromDays(2).ToString()),
            };

            // Act
            var designId = await _designsModule.ExecuteCommandAsync(
                new CreateDesignCommand("Creative Design", 123, designParts));

            // Assert
            var design = await _designsModule.ExecuteQueryAsync(new GetDesignQuery(designId));

            Assert.NotNull(design);
            Assert.Equal("Creative Design", design.Name);
            Assert.Equal(123, design.Price);
            Assert.Equal(2, design.Parts.Count);
        }

        private async Task SetUp(Guid designPartIdFirst, Guid designPartIdSecond)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [designs].[DesignParts] VALUES (@Id, 'Pretty Tie') ", new { Id = designPartIdFirst });
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [designs].[DesignParts] VALUES (@Id, 'Pretty Bow') ", new { Id = designPartIdSecond });
            }
        }
    }
}
