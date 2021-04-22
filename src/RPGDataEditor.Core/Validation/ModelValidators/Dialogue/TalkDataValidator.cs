using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TalkDataValidator : AbstractValidator<TalkData>
    {
        public TalkDataValidator() => RuleFor(x => x.TalkRange).GreaterThan(-1).WithMessage(CustomMessages.Amount);
    }
}
