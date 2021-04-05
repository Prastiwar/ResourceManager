using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class TalkDataModelValidator : Core.Validation.TalkDataModelValidator
    {
        public TalkDataModelValidator() : base()
        {
            TalkLineValidator talkLineValidator = new TalkLineValidator();
            RuleForEach(x => (x as TalkDataModel).DeathLines).SetValidator(talkLineValidator);
            RuleForEach(x => (x as TalkDataModel).HurtLines).SetValidator(talkLineValidator);
            RuleForEach(x => (x as TalkDataModel).InteractLines).SetValidator(talkLineValidator);
        }
    }
}
