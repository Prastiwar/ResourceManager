using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class EquipmentModelValidator : AbstractValidator<EquipmentModel>
    {
        public EquipmentModelValidator()
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
