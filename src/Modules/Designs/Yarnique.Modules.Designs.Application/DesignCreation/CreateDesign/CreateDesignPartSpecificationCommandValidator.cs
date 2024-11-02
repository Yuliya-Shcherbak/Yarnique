using FluentValidation;

namespace Yarnique.Modules.Designs.Application.DesignCreation.CreateDesign
{
    internal class CreateDesignPartSpecificationCommandValidator : AbstractValidator<CreateDesignPartSpecificationCommand>
    {
        public CreateDesignPartSpecificationCommandValidator() 
        {
            RuleFor(x => x.Term).NotEmpty().WithMessage("Term of Design Part execution cannot be empty.");
            RuleFor(x => x.Term).Must(x => TimeSpan.TryParse(x, out _)).WithMessage("Term paroperty has an invalid TimeSpan value.");
        }
    }
}
