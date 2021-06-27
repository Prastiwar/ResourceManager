using FluentValidation;
using ResourceManager.Core.Validation;
using RpgDataEditor.Models;

namespace RpgDataEditor.Validation
{
    public class TalkLineValidator : AbstractValidator<TalkLine>
    {
        public TalkLineValidator() => RuleFor(x => x.Text).NotEmpty().WithMessage(CustomMessages.Empty).When(x => x.SoundId == null);
    }
}
