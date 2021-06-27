using FluentValidation;
using RpgDataEditor.Models;

namespace RpgDataEditor.Validation
{
    public class NpcValidator : AbstractValidator<Npc>
    {
        public NpcValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Npc name should not be empty");

            RuleFor(x => x.Job).SetValidator(new NpcJobValidator());

            TalkDataValidator talkDataValidator = new TalkDataValidator();
            RuleFor(x => x.TalkData).SetValidator(talkDataValidator);
        }

    }
}
