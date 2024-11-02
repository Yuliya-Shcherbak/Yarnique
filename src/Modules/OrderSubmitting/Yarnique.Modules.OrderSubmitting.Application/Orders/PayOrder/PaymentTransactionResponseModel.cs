using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.PayOrder
{
    public class PaymentTransactionResponseModel
    {
        [JsonProperty("transactionId")]
        [JsonPropertyName("transactionId")]
        public Guid TransactionId { get; set; }
        [JsonProperty("statusCode")]
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty("transactionError")]
        [JsonPropertyName("transactionError")]
        public string TransactionError { get; set; }
    }
}
