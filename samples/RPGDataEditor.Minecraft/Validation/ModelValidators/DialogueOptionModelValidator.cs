namespace RPGDataEditor.Minecraft.Validation
{
    public class DialogueOptionModelValidator : Core.Validation.DialogueOptionModelValidator
    {
        public DialogueOptionModelValidator() : base()
        {
            PlayerRequirementModelValidator requirementValidator = new PlayerRequirementModelValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
