using Dapper;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using Yarnique.BackgroundService.Configuration;
using Yarnique.BackgroundService.Models;
using Yarnique.Common.Domain.OrderStatuses;
using System.Reflection;

namespace Yarnique.BackgroundService.Helpers
{
    public class ProcessOrderService : IProcessOrderService
    {
        public readonly BackgroundServiceConfig _config;

        public ProcessOrderService(BackgroundServiceConfig config)
        {
            _config = config;
        }

        public async Task ProcessDueOrders()
        {
            using var sqlConnection = new SqlConnection(_config.ConnectionStrings.YarniqueDB);

            const string sql =
            @$"
                SELECT d.Name AS [{nameof(DueOrderInformation.DesignName)}]
	                , dp.Name AS [{nameof(DueOrderInformation.DesignPartName)}]
                    , u.FirstName + ' ' + u.LastName  AS [{nameof(DueOrderInformation.SellerName)}]
                    , u.Email AS [{nameof(DueOrderInformation.ToEmail)}]
	                , oep.DueDate
	                , oep.Status
                FROM [orders].[Orders] AS o
                JOIN [orders].[Designs] AS d ON o.DesignId = d.Id
                JOIN [users].[Users] AS u ON d.SellerId = u.Id
                JOIN [orders].[OrderExecutionProgress] AS oep ON o.Id = oep.OrderId
                JOIN [orders].[DesignPartSpecifications] AS dps ON oep.DesignPartSpecificationId = dps.Id
                JOIN [orders].[DesignParts] AS dp ON dps.DesignPartId = dp.Id
                WHERE o.Status IN @orderStatuses  
	                AND oep.Status != @executionStatus
	                AND oep.DueDate < @now
                ORDER BY dps.ExecutionOrder
            ";

            var dueOrders = await sqlConnection.QueryAsync<DueOrderInformation>(sql,
                new { 
                    now = DateTime.UtcNow, 
                    orderStatuses = new[] { OrderStatus.InProgress.Value, OrderStatus.Accepted.Value },
                    executionStatus = ExecutionStatus.Completed.Value
                }
            );

            foreach (var order in dueOrders)
            {
                SendEmail(order);
            }
        }
        
        private void SendEmail(DueOrderInformation dueOrder)
        {
            var fromAddress = new MailAddress(_config.EmailConfiguration.FromEmail, _config.EmailConfiguration.FromName);
            var toAddress = new MailAddress(dueOrder.ToEmail, dueOrder.SellerName);

            var htmlTemplate = ReadTemplate("Yarnique.BackgroundService.Templates.ExecutionOverdueTemplate.html");

            const string subject = "Execution of the design part is overdue";

            var body = htmlTemplate
                .Replace("#SellerName#", dueOrder.SellerName)
                .Replace("#DesignName#", dueOrder.DesignName)
                .Replace("#DesignPartName#", dueOrder.DesignPartName)
                .Replace("#Status#", dueOrder.Status)
                .Replace("#DueDate#", dueOrder.DueDate.ToShortDateString());

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromAddress.Address, _config.EmailConfiguration.AppPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtpClient.Send(message);
            }
        }

        private string ReadTemplate(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
