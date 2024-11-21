namespace Yarnique.Payment.API.PaymentProcessing
{
    public class PaymentProcessingResponse
    {
        public Guid TransactionId { get; set; }
        public int StatusCode { get; set; }
        public string TransactionError { get; set; }
    }
}
