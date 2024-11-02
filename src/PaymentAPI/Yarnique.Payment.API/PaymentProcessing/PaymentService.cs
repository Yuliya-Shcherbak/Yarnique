namespace Yarnique.Payment.API.PaymentProcessing
{
    public class PaymentService : IPaymentService
    {
        public async Task<PaymentProcessingResponse> Process(PaymentRequest request)
        {
            var cardIsValid = SimpleCreditCardValidator.ValidateCreditCard(request.CardDetails.CardNumber);

            await Task.Delay(5000);

            return new PaymentProcessingResponse()
            {
                StatusCode = cardIsValid ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest,
                TransactionId = Guid.NewGuid(),
                TransactionError = cardIsValid ? null :
                    "Could not process the transaction. Provided Credit Card information is invalid"
            };
        }
    }
}
