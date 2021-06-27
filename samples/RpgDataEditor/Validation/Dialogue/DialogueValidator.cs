using FluentValidation;
using ResourceManager.Core.Validation;
using RpgDataEditor.Models;

namespace RpgDataEditor.Validation
{
    public class DialogueValidator : AbstractValidator<Dialogue>
    {
        public DialogueValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.Message).NotEmpty().WithMessage(CustomMessages.Empty);

            DialogueOptionValidator optionValidator = new DialogueOptionValidator();
            RuleForEach(x => x.Options).SetValidator(optionValidator);

            RequirementValidator requirementValidator = new RequirementValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
