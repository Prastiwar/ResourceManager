namespace RPGDataEditor.Minecraft.Validation
{
    public class DialogueOptionValidator : Core.Validation.DialogueOptionValidator
    {
        public DialogueOptionValidator() : base()
        {
            RequirementValidator requirementValidator = new RequirementValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
