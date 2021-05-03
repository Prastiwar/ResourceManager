namespace RPGDataEditor.Minecraft.Validation
{
    public class DialogueValidator : Core.Validation.DialogueValidator
    {
        public DialogueValidator() : base()
        {
            DialogueOptionValidator optionValidator = new DialogueOptionValidator();
            RuleForEach(x => x.Options).SetValidator(optionValidator);

            RequirementValidator requirementValidator = new RequirementValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
