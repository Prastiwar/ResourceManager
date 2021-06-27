using FluentValidation;
using ResourceManager.Core.Validation;
using RpgDataEditor.Models;

namespace RpgDataEditor.Validation
{
    public class QuestTaskValidator : AbstractValidator<IQuestTask>
    {
        public QuestTaskValidator()
        {
            RuleFor(x => (x as KillQuestTask).Amount).GreaterThan(0).WithMessage(CustomMessages.Amount).When(x => x is KillQuestTask);

            RuleFor(x => (x as DialogueQuestTask).DialogueId).NotEmpty().WithMessage(CustomMessages.Empty).When(x => x is DialogueQuestTask);

            RuleFor(x => (x as EntityInteractQuestTask).EntityId).NotEmpty().WithMessage(CustomMessages.Empty).When(x => x is EntityInteractQuestTask);

            RuleFor(x => (x as ItemInteractQuestTask).ItemId).NotEmpty()
                                                                .WithMessage(CustomMessages.Empty)
                                                                .When(x => x is ItemInteractQuestTask);
        }
    }
}
