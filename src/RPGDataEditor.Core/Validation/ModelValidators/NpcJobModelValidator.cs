using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class NpcJobModelValidator : AbstractValidator<NpcJobModel>
    {
        public NpcJobModelValidator()
        {
            TradeItemModelValidator itemValidator = new TradeItemModelValidator();
            RuleForEach(x => (x as TraderNpcJobModel).Items).SetValidator(itemValidator).When(x => x is TraderNpcJobModel);
        }
    }
}
