using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class NpcJobModelValidator : AbstractValidator<NpcJob>
    {
        public NpcJobModelValidator()
        {
            TradeItemValidator itemValidator = new TradeItemValidator();
            RuleForEach(x => (x as TraderNpcJob).Items).SetValidator(itemValidator).When(x => x is TraderNpcJob);
        }
    }
}
