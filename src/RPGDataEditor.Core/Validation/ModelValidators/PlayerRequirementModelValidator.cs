using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class PlayerRequirementModelValidator : AbstractValidator<Requirement>
    {
        public PlayerRequirementModelValidator()
        {
            RuleFor(x => (x as DialogueRequirement).DialogueId).NotEmpty().WithMessage(CustomMessages.Empty).When(x => x is DialogueRequirement);

            RuleFor(x => (x as ItemRequirement).ItemId).NotEmpty().WithMessage(CustomMessages.Empty).When(x => x is ItemRequirement);

            RuleFor(x => (x as QuestRequirement).QuestId).NotEmpty().WithMessage(CustomMessages.Empty).When(x => x is QuestRequirement);
            RuleFor(x => (x as QuestRequirement).Stage).Must(x => x != QuestStage.Unknown).WithMessage("This is not valid Stage").When(x => x is QuestRequirement);
        }
    }
}
