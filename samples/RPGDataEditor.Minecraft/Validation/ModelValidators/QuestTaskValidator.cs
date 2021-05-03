using FluentValidation;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class QuestTaskValidator : Core.Validation.QuestTaskValidator
    {
        public QuestTaskValidator() : base()
        {
            RuleFor(x => (x as RPGDataEditor.Models.KillQuestTask).TargetId).ResourceLocation(false)
                                                               .WithMessage(CustomMessages.ResourceLocation)
                                                               .When(x => x is RPGDataEditor.Models.KillQuestTask);

            RuleFor(x => (x as RightItemInteractQuestTask).ItemId).ResourceLocation(false)
                                                                  .WithMessage(CustomMessages.ResourceLocation)
                                                                  .When(x => x is RightItemInteractQuestTask);

            RuleFor(x => (x as RightItemInteractQuestTask).Nbt).Json()
                                                                .WithMessage(Core.Validation.CustomMessages.Json)
                                                                .When(x => x is RightItemInteractQuestTask);
        }
    }
}
