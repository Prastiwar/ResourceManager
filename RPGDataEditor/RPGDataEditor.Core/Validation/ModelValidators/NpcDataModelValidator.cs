using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class NpcDataModelValidator : AbstractValidator<NpcDataModel>
    {
        public NpcDataModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Npc name should not be empty");
            RuleFor(x => x.TextureLocation).NotEmpty().WithMessage("Title cannot be empty")
                                           .Must(x => ValidationExtensions.IsResourceLocation(x) || ValidationExtensions.IsUrl(x))
                                           .WithMessage("This is not valid url or resource location");
            RuleFor(x => x.AmbientSoundLocation).ResourceLocation().WithMessage("This is not valid resource location");
            RuleFor(x => x.DeathSoundLocation).ResourceLocation().WithMessage("This is not valid resource location");
            RuleFor(x => x.HurtSoundLocation).ResourceLocation().WithMessage("This is not valid resource location");
        }
    }
}
