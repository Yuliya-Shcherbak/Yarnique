using FluentValidation;

namespace Yarnique.Modules.OrderSubmitting.Application.Orders.CreateOrder
{
    internal class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.DesignId).NotEmpty().WithMessage("DesignId is required.");
            RuleFor(x => x.ExecutionDate).NotEmpty().WithMessage("Execution date is required.");
        }
    }
}
