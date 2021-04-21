using FluentValidation;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class PlayerRequirementModelValidator : Core.Validation.PlayerRequirementModelValidator
    {
        public PlayerRequirementModelValidator() : base()
        {
            RuleFor(x => (x as ItemRequirement).Item).ResourceLocation(false)
                                                     .WithMessage(CustomMessages.ResourceLocation)
                                                     .When(x => x is ItemRequirement);
                                                     
            RuleFor(x => (x as ItemRequirement).Nbt).Json()
                                                    .WithMessage(Core.Validation.CustomMessages.Json)
                                                    .When(x => x is ItemRequirement req && req.RespectNbt);
        }
    }
}
