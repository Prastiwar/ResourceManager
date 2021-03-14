using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TradeItemModelValidator : AbstractValidator<TradeItemModel>
    {
        public TradeItemModelValidator()
        {
            RuleFor(x => x.Item).ResourceLocation(false).WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => x.Nbt).Json().WithMessage(CustomMessages.Json);
            RuleFor(x => x.Count).GreaterThan(0).WithMessage(CustomMessages.Amount);
        }
    }
}
