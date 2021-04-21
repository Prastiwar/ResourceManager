using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TalkLineValidator : AbstractValidator<TalkLine>
    {
        public TalkLineValidator() => RuleFor(x => x.Text).NotEmpty().WithMessage(CustomMessages.Empty).When(x => x.SoundId == null);
    }
}
