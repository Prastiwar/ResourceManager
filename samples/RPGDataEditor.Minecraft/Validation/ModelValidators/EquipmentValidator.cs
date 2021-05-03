using FluentValidation;
using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class EquipmentValidator : AbstractValidator<Equipment>
    {
        public EquipmentValidator()
        {
            RuleFor(x => x.MainHand).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => x.OffHand).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => x.Head).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => x.Chest).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => x.Legs).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => x.Feet).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
        }
    }
}
