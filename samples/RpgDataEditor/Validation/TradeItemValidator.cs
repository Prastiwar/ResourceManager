using FluentValidation;
using ResourceManager.Core.Validation;
using RpgDataEditor.Models;

namespace RpgDataEditor.Validation
{
    public class TradeItemValidator : AbstractValidator<TradeItem>
    {
        public TradeItemValidator()
        {
            RuleFor(x => x.ItemId).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.Count).GreaterThan(0).WithMessage(CustomMessages.Amount);
        }
    }
}
