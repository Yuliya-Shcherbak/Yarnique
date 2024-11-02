using Yarnique.Modules.OrderSubmitting.Application.Configuration.Commands;
using Yarnique.Modules.OrderSubmitting.Domain.Orders;
using Newtonsoft.Json;
using System.Text;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.PayOrder
{
    internal class PayOrderCommandHandler : ICommandHandler<PayOrderCommand>
    {
        private readonly string HttpClientName = "PaymentApiClient";
        private readonly IOrderSubmittingRepository _ordersRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public PayOrderCommandHandler(IOrderSubmittingRepository ordersRepository, IHttpClientFactory httpClientFactory)
        {
            _ordersRepository = ordersRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Handle(PayOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _ordersRepository.GetByIdAsync(command.OrderId);

            using var httpClient = _httpClientFactory.CreateClient(HttpClientName);
            PaymentTransactionRequestModel requestBody = new()
            {
                Amount = 200,
                CardDetails = new() { CardholderName = command.CardholderName, CardNumber = command.CardNumber },
                SellerInfo = new() { SellerId = Guid.NewGuid(), Email = "" },
                TransactionMetadata = new() { TransactionId = $"TR-{Guid.NewGuid()}", Description = $"Payment for design production" },
            };
            var jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var apiResponse = await httpClient.PostAsync("api/payment", content);

            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<PaymentTransactionResponseModel>(responseContent);
            order.AddPaymentInfo(
                apiResponse.IsSuccessStatusCode, response.TransactionId.ToString(), response.TransactionError);
        }
    }
}
