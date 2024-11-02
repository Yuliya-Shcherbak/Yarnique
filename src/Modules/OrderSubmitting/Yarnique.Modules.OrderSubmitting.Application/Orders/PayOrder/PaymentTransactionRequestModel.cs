using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.PayOrder
{
    public class PaymentTransactionRequestModel
    {
        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        [JsonProperty("cardDetails")]
        [JsonPropertyName("cardDetails")]
        public CardDetails CardDetails { get; set; }
        [JsonProperty("sellerInfo")]
        [JsonPropertyName("sellerInfo")]
        public SellerInfo SellerInfo { get; set; }
        [JsonProperty("transactionMetadata")]
        [JsonPropertyName("transactionMetadata")]
        public TransactionMetadata TransactionMetadata { get; set; }
    }

    public class CardDetails
    {
        [JsonProperty("cardNumber")]
        [JsonPropertyName("cardNumber")]
        public string CardNumber { get; set; }
        [JsonProperty("cardholderName")]
        [JsonPropertyName("cardholderName")]
        public string CardholderName { get; set; }
    }

    public class SellerInfo
    {
        [JsonProperty("customerId")]
        [JsonPropertyName("customerId")]
        public Guid SellerId { get; set; }
        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class TransactionMetadata
    {
        [JsonProperty("transactionId")]
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; }
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
