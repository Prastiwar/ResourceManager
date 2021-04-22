using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TalkDataModelValidator : AbstractValidator<TalkData>
    {
        public TalkDataModelValidator() => RuleFor(x => x.TalkRange).GreaterThan(-1).WithMessage(CustomMessages.Amount);
    }
}
