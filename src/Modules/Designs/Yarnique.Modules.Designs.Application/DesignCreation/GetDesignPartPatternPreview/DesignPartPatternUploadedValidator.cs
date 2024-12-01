using FluentValidation;

namespace Yarnique.Modules.Designs.Application.DesignCreation.GetDesignPartPatternPreview
{
    internal class DesignPartPatternUploadedValidator : AbstractValidator<DesignPartBlobNameDto>
    {
        public DesignPartPatternUploadedValidator()
        {
            RuleFor(x => x.BlobName).NotEmpty().WithMessage("No pattern for Design Part has been uploaded.");
        }
    }
}
