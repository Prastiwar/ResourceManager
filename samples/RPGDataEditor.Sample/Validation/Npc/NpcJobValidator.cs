using FluentValidation;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.Validation
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
