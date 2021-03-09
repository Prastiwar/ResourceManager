using FluentValidation;
using RPGDataEditor.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace RPGDataEditor.Core.Validation
{
    public class NpcDataModelValidator : AbstractValidator<NpcDataModel>
    {
        public NpcDataModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Npc name should not be empty");
            RuleFor(x => x.TextureLocation).Must(x => ValidationExtensions.IsResourceLocation(x) || ValidationExtensions.IsUrl(x))
                                           .WithMessage("This is not valid url or resource location");

            RuleFor(x => x.AmbientSoundLocation).ResourceLocation().WithMessage("This is not valid resource location");
            RuleFor(x => x.DeathSoundLocation).ResourceLocation().WithMessage("This is not valid resource location");
            RuleFor(x => x.HurtSoundLocation).ResourceLocation().WithMessage("This is not valid resource location");

            RuleFor(x => x.Job).Must(IsValidTrader).WithMessage("At least one Trade Item is not valid").When(x => x.Job is TraderNpcJobModel);

            RuleFor(x => x.TalkData.DeathLines).Must(ValidTalkLines).WithMessage("At least one Talk Line is not valid");
            RuleFor(x => x.TalkData.HurtLines).Must(ValidTalkLines).WithMessage("At least one Talk Line is not valid");
            RuleFor(x => x.TalkData.InteractLines).Must(ValidTalkLines).WithMessage("At least one Talk Line is not valid");
        }

        private bool ValidTalkLines(IList<TalkLine> lines) => !lines.Any(line => !ValidationExtensions.IsResourceLocation(line.SoundLocation));

        private bool IsValidTrader(NpcJobModel job)
        {
            if (job is TraderNpcJobModel traderJob)
            {
                return !traderJob.Items.Any(item => !ValidationExtensions.IsResourceLocation(item.Item));
            }
            return false;
        }
    }
}
