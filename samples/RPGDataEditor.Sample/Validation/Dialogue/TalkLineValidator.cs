using FluentValidation;
using RPGDataEditor.Core.Validation;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Validation
{
    public class TalkLineValidator : AbstractValidator<TalkLine>
    {
        public TalkLineValidator() => RuleFor(x => x.Text).NotEmpty().WithMessage(CustomMessages.Empty).When(x => x.SoundId == null);
    }
}
