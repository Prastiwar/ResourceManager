using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class DialogueOptionModelValidator : AbstractValidator<DialogueOptionModel>
    {
        public DialogueOptionModelValidator()
        {
            RuleFor(x => x.Message).NotEmpty().WithMessage(CustomMessages.Empty);

            PlayerRequirementModelValidator requirementValidator = new PlayerRequirementModelValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
