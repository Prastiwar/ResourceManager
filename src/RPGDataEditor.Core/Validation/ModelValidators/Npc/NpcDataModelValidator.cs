using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class NpcValidator : AbstractValidator<Npc>
    {
        public NpcValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Npc name should not be empty");

            RuleFor(x => x.Job).SetValidator(new NpcJobModelValidator());

            TalkDataValidator talkDataValidator = new TalkDataValidator();
            RuleFor(x => x.TalkData).SetValidator(talkDataValidator);
        }

    }
}
