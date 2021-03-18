using FluentValidation;

namespace RPGDataEditor.Minecraft.Validation
{
    public class QuestModelValidator : Core.Validation.QuestModelValidator
    {
        public QuestModelValidator() : base()
        {
            QuestTaskValidator taskValidator = new QuestTaskValidator();
            RuleFor(x => x.CompletionTask).NotEmpty().WithMessage(Core.Validation.CustomMessages.Empty)
                                          .SetValidator(taskValidator);

            RuleForEach(x => x.Tasks).SetValidator(taskValidator);

            PlayerRequirementModelValidator requirementValidator = new PlayerRequirementModelValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
