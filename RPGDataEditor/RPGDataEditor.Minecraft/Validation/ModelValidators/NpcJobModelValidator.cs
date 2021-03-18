using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Minecraft.Validation
{
    public class NpcJobModelValidator : Core.Validation.NpcJobModelValidator
    {
        public NpcJobModelValidator() : base()
        {
            TradeItemModelValidator itemValidator = new TradeItemModelValidator();
            RuleForEach(x => (x as TraderNpcJobModel).Items).SetValidator(itemValidator).When(x => x is TraderNpcJobModel);
        }
    }
}
