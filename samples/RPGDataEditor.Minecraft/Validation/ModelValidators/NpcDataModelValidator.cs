using FluentValidation;
using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class NpcDataModelValidator : Core.Validation.NpcDataModelValidator
    {
        public NpcDataModelValidator() : base()
        {
            RuleFor(x => (x as NpcDataModel).TextureLocation).Must(x => ValidationExtensions.IsResourceLocation(x) || Core.ValidationExtensions.IsUrl(x))
                                           .WithMessage("This is not valid url or resource location");

            RuleFor(x => (x as NpcDataModel).AmbientSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => (x as NpcDataModel).DeathSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => (x as NpcDataModel).HurtSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);

            RuleFor(x => x.Job).SetValidator(new NpcJobModelValidator());

            TalkDataModelValidator talkDataValidator = new TalkDataModelValidator();
            RuleFor(x => x.TalkData).SetValidator(talkDataValidator);

            EquipmentModelValidator equipmentValidator = new EquipmentModelValidator();
            RuleFor(x => (x as NpcDataModel).Equipment).SetValidator(equipmentValidator);
        }

    }
}
