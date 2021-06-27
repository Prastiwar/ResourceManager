using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class TalkDataValidator : Core.Validation.TalkDataValidator
    {
        public TalkDataValidator() : base()
        {
            TalkLineValidator talkLineValidator = new TalkLineValidator();
            RuleForEach(x => (x as TalkData).DeathLines).SetValidator(talkLineValidator);
            RuleForEach(x => (x as TalkData).HurtLines).SetValidator(talkLineValidator);
            RuleForEach(x => (x as TalkData).InteractLines).SetValidator(talkLineValidator);
        }
    }
}
