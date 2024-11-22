namespace Yarnique.Payment.API.PaymentProcessing
{
    public class PaymentRequest
    {
        public double Amount { get; set; }
        public CardDetails CardDetails { get; set; }
        public SellerInfo SellerInfo { get; set; }
        public TransactionMetadata TransactionMetadata { get; set; }
    }

    public class CardDetails
    {
        public string CardNumber { get; set; }
        public string CardholderName { get; set; }
    }

    public class SellerInfo
    {
        public Guid CustomerId { get; set; }
        public string Email { get; set; }
    }

    public class TransactionMetadata
    {
        public string TransactionId { get; set; }
        public string Description { get; set; }
    }
}
