using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class TalkDataModelValidator : AbstractValidator<TalkDataModel>
    {
        public TalkDataModelValidator()
        {
            TalkLineValidator talkLineValidator = new TalkLineValidator();
            RuleFor(x => x.TalkRange).GreaterThan(-1).WithMessage(CustomMessages.Amount);
            RuleForEach(x => x.DeathLines).SetValidator(talkLineValidator);
            RuleForEach(x => x.HurtLines).SetValidator(talkLineValidator);
            RuleForEach(x => x.InteractLines).SetValidator(talkLineValidator);
        }
    }
}
