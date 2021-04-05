using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TradeItemModelValidator : AbstractValidator<TradeItemModel>
    {
        public TradeItemModelValidator()
        {
            RuleFor(x => x.Item).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.Count).GreaterThan(0).WithMessage(CustomMessages.Amount);
        }
    }
}
