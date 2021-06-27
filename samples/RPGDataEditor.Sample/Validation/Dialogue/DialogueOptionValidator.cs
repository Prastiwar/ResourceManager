using FluentValidation;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Validation
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
