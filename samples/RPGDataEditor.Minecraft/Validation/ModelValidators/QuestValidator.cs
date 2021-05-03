using FluentValidation;

namespace RPGDataEditor.Minecraft.Validation
{
    public class QuestValidator : Core.Validation.QuestValidator
    {
        public QuestValidator() : base()
        {
            QuestTaskValidator taskValidator = new QuestTaskValidator();
            RuleFor(x => x.CompletionTask).NotEmpty().WithMessage(Core.Validation.CustomMessages.Empty)
                                          .SetValidator(taskValidator);

            RuleForEach(x => x.Tasks).SetValidator(taskValidator);

            RequirementValidator requirementValidator = new RequirementValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
