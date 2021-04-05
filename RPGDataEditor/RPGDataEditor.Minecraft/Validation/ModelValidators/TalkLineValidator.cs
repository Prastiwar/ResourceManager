using FluentValidation;

namespace RPGDataEditor.Minecraft.Validation
{
    public class TalkLineValidator : Core.Validation.TalkLineValidator
    {
        public TalkLineValidator() => RuleFor(x => x.SoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
    }
}
