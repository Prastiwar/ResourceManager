using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class NpcJobValidator : Core.Validation.NpcJobValidator
    {
        public NpcJobValidator() : base()
        {
            TradeItemValidator itemValidator = new TradeItemValidator();
            RuleForEach(x => (x as TraderNpcJob).Items).SetValidator(itemValidator).When(x => x is TraderNpcJob);
        }
    }
}
