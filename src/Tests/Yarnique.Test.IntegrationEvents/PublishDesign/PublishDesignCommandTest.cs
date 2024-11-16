using Dapper;
using System.Data.SqlClient;
using Yarnique.Modules.Designs.Application.Contracts;
using Yarnique.Modules.Designs.Application.DesignCreation.GetDesign;
using Yarnique.Modules.Designs.Application.DesignCreation.PublishDesign;
using Yarnique.Modules.OrderSubmitting.Application.Contracts;
using Yarnique.Modules.OrderSubmitting.Application.Designs.GetDesigns;
using Yarnique.Test.Common.Probing;
using DesignDto = Yarnique.Modules.Designs.Application.DesignCreation.GetDesign.DesignDto;
using DesignsDto = Yarnique.Modules.OrderSubmitting.Application.Designs.GetDesigns.DesignDto;

namespace Yarnique.Test.Integration.PublishDesign
{
    public class PublishDesignCommandTest : TestBase
    {
        [Fact]
        public async Task PublishDesignCommandHandler_ShouldPublishDesignToOrdersSchema()
        {
            // Arrange
            var designId = Guid.NewGuid();
            var designName = "Blue Bear";
            var designPartIdFirst = Guid.NewGuid();
            var designPartIdSecond = Guid.NewGuid();
            await AddDesign(designId, designName, designPartIdFirst, designPartIdSecond);

            var publishCommand = new PublishDesignCommand(designId);

            // Act
            await _designsModule.ExecuteCommandAsync(publishCommand);

            // Assert
            await AssertEventually(new GetPublishedDesignFromDesignsProbe(designId, _designsModule), 10000);

            await AssertEventually(new GetPublishedDesignFromOrdersProbe(designName, _ordersModule), 15000);
        }

        private async Task AddDesign(Guid designId, string designName, Guid designPartIdFirst, Guid designPartIdSecond)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [designs].[DesignParts] VALUES (@Id, 'Left Arm') ", new { Id = designPartIdFirst });
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [designs].[DesignParts] VALUES (@Id, 'Right Arm') ", new { Id = designPartIdSecond });
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [designs].[Designs] VALUES (@Id, @Name, 120, 0, @SellerId) ", new { Id = designId, Name = designName, SellerId = Guid.NewGuid() });
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [designs].[DesignPartSpecifications] VALUES (@Id, @DesignId, @DesignPartId, 80, '1.00:00:00', 1) ",
                    new { Id = Guid.NewGuid(), DesignId = designId, DesignPartId = designPartIdFirst });
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [designs].[DesignPartSpecifications] VALUES (@Id, @DesignId, @DesignPartId, 50, '1.00:00:00', 2) ",
                    new { Id = Guid.NewGuid(), DesignId = designId, DesignPartId = designPartIdSecond });
            }
        }

        private class GetPublishedDesignFromDesignsProbe : IProbe
        {
            private readonly IDesignsModule _designsModule;

            private Guid _expectedDesignId;

            private DesignDto _design;

            public GetPublishedDesignFromDesignsProbe(Guid expectedDesignId, IDesignsModule designsModule)
            {
                _expectedDesignId = expectedDesignId;
                _designsModule = designsModule;
            }

            public bool IsSatisfied()
            {
                return _design != null && _design.IsPublished;
            }

            public async Task SampleAsync()
            {
                _design = await _designsModule.ExecuteQueryAsync(new GetDesignQuery(_expectedDesignId));
            }

            public string DescribeFailureTo()
                => $"Design with ID: {_expectedDesignId} is not published";
        }

        private class GetPublishedDesignFromOrdersProbe : IProbe
        {
            private readonly IOrderSubmittingModule _ordersModule;

            private string _expectedDesignName;

            private List<DesignsDto> _designs;

            public GetPublishedDesignFromOrdersProbe(string expectedDesignName, IOrderSubmittingModule meetingsModule)
            {
                _expectedDesignName = expectedDesignName;
                _ordersModule = meetingsModule;
            }

            public bool IsSatisfied()
            {
                return _designs != null && _designs.Any(x => x.Name == _expectedDesignName);
            }

            public async Task SampleAsync()
            {
                _designs = (await _ordersModule.ExecuteQueryAsync(new GetDesignsQuery(1, 5))).Items;
            }

            public string DescribeFailureTo()
                => $"Design with Name: {_expectedDesignName} is not published";
        }
    }
}
