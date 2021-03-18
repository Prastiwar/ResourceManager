using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class TalkDataModelValidator : AbstractValidator<TalkDataModel>
    {
        public TalkDataModelValidator() : base()
        {
            TalkLineValidator talkLineValidator = new TalkLineValidator();
            RuleForEach(x => x.DeathLines).SetValidator(talkLineValidator);
            RuleForEach(x => x.HurtLines).SetValidator(talkLineValidator);
            RuleForEach(x => x.InteractLines).SetValidator(talkLineValidator);
        }
    }
}
