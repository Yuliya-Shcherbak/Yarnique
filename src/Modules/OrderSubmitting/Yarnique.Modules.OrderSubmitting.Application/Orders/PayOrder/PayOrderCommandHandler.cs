using Yarnique.Modules.OrderSubmitting.Application.Configuration.Commands;
using Yarnique.Modules.OrderSubmitting.Domain.Orders;
using Newtonsoft.Json;
using System.Text;
using RestSharp;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.PayOrder
{
    internal class PayOrderCommandHandler : ICommandHandler<PayOrderCommand>
    {
        private readonly IOrderSubmittingRepository _ordersRepository;
        private readonly RestClient _restClient;

        public PayOrderCommandHandler(IOrderSubmittingRepository ordersRepository, RestClient restClient)
        {
            _ordersRepository = ordersRepository;
            _restClient = restClient;
        }

        public async Task Handle(PayOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _ordersRepository.GetByIdAsync(command.OrderId);

            PaymentTransactionRequestModel requestBody = new()
            {
                Amount = 200,
                CardDetails = new() { CardholderName = command.CardholderName, CardNumber = command.CardNumber },
                SellerInfo = new() { SellerId = Guid.NewGuid(), Email = "" },
                TransactionMetadata = new() { TransactionId = $"TR-{Guid.NewGuid()}", Description = $"Payment for design production" },
            };

            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var request = new RestRequest("api/payment", Method.Post)
                .AddStringBody(jsonContent, ContentType.Json);

            var response = await _restClient.PostAsync<PaymentTransactionResponseModel>(request, cancellationToken);

            order.AddPaymentInfo(
                string.IsNullOrEmpty(response.TransactionError), response.TransactionId.ToString(), response.TransactionError);
        }
    }
}
