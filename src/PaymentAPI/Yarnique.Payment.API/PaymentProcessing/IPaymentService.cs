namespace Yarnique.Payment.API.PaymentProcessing
{
    public interface IPaymentService
    {
        Task<PaymentProcessingResponse> Process(PaymentRequest request);
    }
}
