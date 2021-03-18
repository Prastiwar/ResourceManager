using FluentValidation;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class TradeItemModelValidator : Core.Validation.TradeItemModelValidator
    {
        public TradeItemModelValidator() : base()
        {
            RuleFor(x => (x as TradeItemModel).Item).ResourceLocation(false).WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => (x as TradeItemModel).Nbt).Json().WithMessage(Core.Validation.CustomMessages.Json);
        }
    }
}
