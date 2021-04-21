using FluentValidation;
using RPGDataEditor.Models;

namespace RPGDataEditor.Core.Validation
{
    public class NpcDataModelValidator : AbstractValidator<NpcDataModel>
    {
        public NpcDataModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Npc name should not be empty");

            RuleFor(x => x.Job).SetValidator(new NpcJobModelValidator());

            TalkDataModelValidator talkDataValidator = new TalkDataModelValidator();
            RuleFor(x => x.TalkData).SetValidator(talkDataValidator);
        }

    }
}
