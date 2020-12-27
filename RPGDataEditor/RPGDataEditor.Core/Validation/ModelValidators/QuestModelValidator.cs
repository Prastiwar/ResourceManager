using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class QuestModelValidator : AbstractValidator<QuestModel>
    {
        public QuestModelValidator()
        {
            RuleFor(x => x.CompletionTask).NotEmpty().WithMessage("Quest needs task that will complete quest");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title cannot be empty");
        }
    }
}
