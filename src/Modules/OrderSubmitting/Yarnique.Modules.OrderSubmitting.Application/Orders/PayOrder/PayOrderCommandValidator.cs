using FluentValidation;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.PayOrder
{
    public class PayOrderCommandValidator : AbstractValidator<PayOrderCommand>
    {
        public PayOrderCommandValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty().WithMessage("Card number is required.");
            RuleFor(x => x.CardholderName).NotEmpty().WithMessage("Cardholder Name date is required.");
        }
    }
}
