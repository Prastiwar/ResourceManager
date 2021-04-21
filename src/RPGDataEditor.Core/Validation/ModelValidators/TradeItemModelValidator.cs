using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TradeItemModelValidator : AbstractValidator<TradeItemModel>
    {
        public TradeItemModelValidator()
        {
            RuleFor(x => x.ItemId).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.Count).GreaterThan(0).WithMessage(CustomMessages.Amount);
        }
    }
}
