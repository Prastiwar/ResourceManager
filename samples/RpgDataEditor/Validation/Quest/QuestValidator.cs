using FluentValidation;
using ResourceManager.Core.Validation;
using RpgDataEditor.Models;

namespace RpgDataEditor.Validation
{
    public class QuestValidator : AbstractValidator<Quest>
    {
        public QuestValidator()
        {
            RuleFor(x => x.CompletionTask).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.Title).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.Message).NotEmpty().WithMessage(CustomMessages.Empty);

            QuestTaskValidator taskValidator = new QuestTaskValidator();
            RuleFor(x => x.CompletionTask).NotEmpty().WithMessage(CustomMessages.Empty)
                                          .SetValidator(taskValidator);

            RuleForEach(x => x.Tasks).SetValidator(taskValidator);

            RequirementValidator requirementValidator = new RequirementValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
