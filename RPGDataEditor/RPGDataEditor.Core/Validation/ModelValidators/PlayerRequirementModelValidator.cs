using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class PlayerRequirementModelValidator : AbstractValidator<PlayerRequirementModel>
    {
        public PlayerRequirementModelValidator()
        {
            RuleFor(x => (x as DialogueRequirement).DialogueId).GreaterThan(-1).WithMessage(CustomMessages.Id).When(x => x is DialogueRequirement);

            RuleFor(x => (x as ItemRequirement).Item).ResourceLocation(false).WithMessage(CustomMessages.ResourceLocation).When(x => x is ItemRequirement);

            RuleFor(x => (x as ItemRequirement).Nbt).Json().WithMessage(CustomMessages.Json).When(x => x is ItemRequirement req && req.RespectNbt);

            RuleFor(x => (x as QuestRequirement).QuestId).GreaterThan(-1).WithMessage(CustomMessages.Id).When(x => x is QuestRequirement);
            RuleFor(x => (x as QuestRequirement).Stage).Must(x => x != QuestStage.UNKNOWN).WithMessage("This is not valid Stage").When(x => x is QuestRequirement);
        }
    }
}
