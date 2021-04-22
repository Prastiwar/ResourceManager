using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class DialogueOptionModelValidator : AbstractValidator<DialogueOption>
    {
        public DialogueOptionModelValidator()
        {
            RuleFor(x => x.Message).NotEmpty().WithMessage(CustomMessages.Empty);

            PlayerRequirementModelValidator requirementValidator = new PlayerRequirementModelValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
