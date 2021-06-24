using FluentValidation;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Validation
{
    public class TalkDataValidator : AbstractValidator<TalkData>
    {
        public TalkDataValidator() => RuleFor(x => x.TalkRange).GreaterThan(-1).WithMessage(CustomMessages.Amount);
    }
}
