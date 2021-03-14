using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TalkDataModelValidator : AbstractValidator<TalkDataModel>
    {
        public TalkDataModelValidator()
        {
            TalkLineValidator talkLineValidator = new TalkLineValidator();
            RuleForEach(x => x.DeathLines).SetValidator(talkLineValidator);
            RuleForEach(x => x.HurtLines).SetValidator(talkLineValidator);
            RuleForEach(x => x.InteractLines).SetValidator(talkLineValidator);
        }
    }
}
