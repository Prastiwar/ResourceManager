using FluentValidation;
using ResourceManager.Core.Validation;
using RpgDataEditor.Models;

namespace RpgDataEditor.Validation
{
    public class TalkDataValidator : AbstractValidator<TalkData>
    {
        public TalkDataValidator() => RuleFor(x => x.TalkRange).GreaterThan(-1).WithMessage(CustomMessages.Amount);
    }
}
