using FluentValidation;
using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class NpcValidator : Core.Validation.NpcValidator
    {
        public NpcValidator() : base()
        {
            RuleFor(x => (x as Npc).TextureLocation).Must(x => ValidationExtensions.IsResourceLocation(x) || Core.ValidationExtensions.IsUrl(x))
                                           .WithMessage("This is not valid url or resource location");

            RuleFor(x => (x as Npc).AmbientSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => (x as Npc).DeathSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => (x as Npc).HurtSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);

            RuleFor(x => x.Job).SetValidator(new NpcJobValidator());

            TalkDataValidator talkDataValidator = new TalkDataValidator();
            RuleFor(x => x.TalkData).SetValidator(talkDataValidator);

            EquipmentValidator equipmentValidator = new EquipmentValidator();
            RuleFor(x => (x as Npc).Equipment).SetValidator(equipmentValidator);
        }

    }
}
