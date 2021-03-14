using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TalkLineValidator : AbstractValidator<TalkLine>
    {
        public TalkLineValidator()
        {
            RuleFor(x => x.Text).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.SoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
        }
    }
}
