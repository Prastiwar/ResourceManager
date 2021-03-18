using FluentValidation;
using RPGDataEditor.Core;
using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class TalkLineValidator : Core.Validation.TalkLineValidator
    {
        public TalkLineValidator() : base()
            => RuleFor(x => (x as TalkLine).SoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
    }
}
