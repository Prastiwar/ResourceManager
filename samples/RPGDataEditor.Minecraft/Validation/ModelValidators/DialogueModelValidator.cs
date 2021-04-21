namespace RPGDataEditor.Minecraft.Validation
{
    public class DialogueModelValidator : Core.Validation.DialogueModelValidator
    {
        public DialogueModelValidator() : base()
        {
            DialogueOptionModelValidator optionValidator = new DialogueOptionModelValidator();
            RuleForEach(x => x.Options).SetValidator(optionValidator);

            PlayerRequirementModelValidator requirementValidator = new PlayerRequirementModelValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
