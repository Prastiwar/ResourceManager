using FluentValidation;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class RequirementValidator : Core.Validation.RequirementValidator
    {
        public RequirementValidator() : base()
        {
            RuleFor(x => (x as ItemRequirement).ItemId).ResourceLocation(false)
                                                       .WithMessage(CustomMessages.ResourceLocation)
                                                       .When(x => x is ItemRequirement);

            RuleFor(x => (x as ItemRequirement).Nbt).Json()
                                                    .WithMessage(Core.Validation.CustomMessages.Json)
                                                    .When(x => x is ItemRequirement req && req.RespectNbt);
        }
    }
}
