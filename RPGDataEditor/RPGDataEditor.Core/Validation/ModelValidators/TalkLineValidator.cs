using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TalkLineValidator : AbstractValidator<TalkLine>
    {
        public TalkLineValidator() => RuleFor(x => x.SoundLocation).ResourceLocation().WithMessage("This is not valid resource location");
    }
}
