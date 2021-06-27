using FluentValidation;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class TradeItemValidator : Core.Validation.TradeItemValidator
    {
        public TradeItemValidator() : base()
        {
            RuleFor(x => (x as TradeItem).ItemId).ResourceLocation(false).WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => (x as TradeItem).Nbt).Json().WithMessage(Core.Validation.CustomMessages.Json);
        }
    }
}
