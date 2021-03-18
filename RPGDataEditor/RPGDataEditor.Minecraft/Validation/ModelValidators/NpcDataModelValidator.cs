using FluentValidation;
using RPGDataEditor.Core;

namespace RPGDataEditor.Minecraft.Validation
{
    public class NpcDataModelValidator : Core.Validation.NpcDataModelValidator
    {
        public NpcDataModelValidator() : base()
        {
            RuleFor(x => x.TextureLocation).Must(x => ValidationExtensions.IsResourceLocation(x) || ValidationExtensions.IsUrl(x))
                                           .WithMessage("This is not valid url or resource location");

            RuleFor(x => x.AmbientSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => x.DeathSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => x.HurtSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);

            RuleFor(x => x.Job).SetValidator(new NpcJobModelValidator());

            TalkDataModelValidator talkDataValidator = new TalkDataModelValidator();
            RuleFor(x => x.TalkData).SetValidator(talkDataValidator);
        }

    }
}
