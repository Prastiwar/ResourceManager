using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TalkDataModelValidator : AbstractValidator<TalkDataModel>
    {
        public TalkDataModelValidator() => RuleFor(x => x.TalkRange).GreaterThan(-1).WithMessage(CustomMessages.Amount);
    }
}
