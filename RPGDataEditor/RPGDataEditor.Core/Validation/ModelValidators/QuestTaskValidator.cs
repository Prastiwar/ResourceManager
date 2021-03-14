using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class QuestTaskValidator : AbstractValidator<QuestTask>
    {
        public QuestTaskValidator()
        {
            RuleFor(x => (x as KillQuestTask).Kill).ResourceLocation(false).WithMessage(CustomMessages.ResourceLocation).When(x => x is KillQuestTask);
            RuleFor(x => (x as KillQuestTask).Amount).GreaterThan(0).WithMessage(CustomMessages.Amount).When(x => x is KillQuestTask);

            RuleFor(x => (x as DialogueQuestTask).DialogueId).GreaterThan(-1).WithMessage(CustomMessages.Id).When(x => x is DialogueQuestTask);

            RuleFor(x => (x as EntityInteractQuestTask).Entity).GreaterThan(-1).WithMessage(CustomMessages.Id).When(x => x is EntityInteractQuestTask);

            RuleFor(x => (x as RightItemInteractQuestTask).Item).ResourceLocation(false)
                                                                .WithMessage(CustomMessages.ResourceLocation)
                                                                .When(x => x is RightItemInteractQuestTask);

            RuleFor(x => (x as RightItemInteractQuestTask).Nbt).Json()
                                                                .WithMessage(CustomMessages.Json)
                                                                .When(x => x is RightItemInteractQuestTask);
        }
    }
}
