using FluentValidation;
using ResourceManager.Core.Validation;
using RpgDataEditor.Models;

namespace RpgDataEditor.Validation
{
    public class DialogueOptionValidator : AbstractValidator<DialogueOption>
    {
        public DialogueOptionValidator()
        {
            RuleFor(x => x.Message).NotEmpty().WithMessage(CustomMessages.Empty);

            RequirementValidator requirementValidator = new RequirementValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
