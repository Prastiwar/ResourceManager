using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class QuestTaskValidator : AbstractValidator<QuestTask>
    {
        public QuestTaskValidator()
        {
            RuleFor(x => ((KillQuestTask)x).Kill).ResourceLocation().WithMessage("This is not valid resource location").When(x => x is KillQuestTask);

            RuleFor(x => ((RightItemInteractQuestTask)x).Item).ResourceLocation().WithMessage("This is not valid resource location").When(x => x is RightItemInteractQuestTask);
            RuleFor(x => ((RightItemInteractQuestTask)x).Nbt).Json().WithMessage("Nbt should be json formatted").When(x => x is RightItemInteractQuestTask);
        }
    }
}
