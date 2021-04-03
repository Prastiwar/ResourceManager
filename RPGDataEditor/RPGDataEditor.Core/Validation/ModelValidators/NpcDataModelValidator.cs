using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class NpcDataModelValidator : AbstractValidator<NpcDataModel>
    {
        public NpcDataModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Npc name should not be empty");
            RuleFor(x => x.TextureLocation).Must(x => ValidationExtensions.IsResourceLocation(x) || ValidationExtensions.IsUrl(x))
                                           .WithMessage("This is not valid url or resource location");

            RuleFor(x => x.AmbientSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => x.DeathSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);
            RuleFor(x => x.HurtSoundLocation).ResourceLocation().WithMessage(CustomMessages.ResourceLocation);

            RuleFor(x => x.Job).SetValidator(new NpcJobModelValidator());

            TalkDataModelValidator talkDataValidator = new TalkDataModelValidator();
            RuleFor(x => x.TalkData).SetValidator(talkDataValidator);

            EquipmentModelValidator equipmentValidator = new EquipmentModelValidator();
            RuleFor(x => x.Equipment).SetValidator(equipmentValidator);
        }

    }
}
