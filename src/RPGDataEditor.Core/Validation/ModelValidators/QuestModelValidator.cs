using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class QuestModelValidator : AbstractValidator<QuestModel>
    {
        public QuestModelValidator()
        {
            RuleFor(x => x.CompletionTask).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.Title).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.Message).NotEmpty().WithMessage(CustomMessages.Empty);

            QuestTaskValidator taskValidator = new QuestTaskValidator();
            RuleFor(x => x.CompletionTask).NotEmpty().WithMessage(CustomMessages.Empty)
                                          .SetValidator(taskValidator);

            RuleForEach(x => x.Tasks).SetValidator(taskValidator);

            PlayerRequirementModelValidator requirementValidator = new PlayerRequirementModelValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
