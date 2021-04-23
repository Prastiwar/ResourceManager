using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class NpcJobValidator : AbstractValidator<NpcJob>
    {
        public NpcJobValidator()
        {
            TradeItemValidator itemValidator = new TradeItemValidator();
            RuleForEach(x => (x as TraderNpcJob).Items).SetValidator(itemValidator).When(x => x is TraderNpcJob);
        }
    }
}
