using FluentValidation;

namespace RPGDataEditor.Minecraft.Validation
{
    public class TalkLineValidator : Core.Validation.TalkLineValidator
    {
        public TalkLineValidator() => RuleFor(x => x.SoundId).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
    }
}
