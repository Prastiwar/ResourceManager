using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TradeItemModelValidator : AbstractValidator<TradeItemModel>
    {
        public TradeItemModelValidator()
        {
            RuleFor(x => x.Item).NotEmpty().WithMessage("Item cannot be empty")
                                .ResourceLocation().WithMessage("This is not valid resource location");

            RuleFor(x => x.Nbt).Json().WithMessage("Nbt should be json formatted");
        }
    }
}
