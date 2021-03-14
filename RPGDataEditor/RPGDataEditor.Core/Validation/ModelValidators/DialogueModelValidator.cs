﻿using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class DialogueModelValidator : AbstractValidator<DialogueModel>
    {
        public DialogueModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.Message).NotEmpty().WithMessage(CustomMessages.Empty);

            DialogueOptionModelValidator optionValidator = new DialogueOptionModelValidator();
            RuleForEach(x => x.Options).SetValidator(optionValidator);

            PlayerRequirementModelValidator requirementValidator = new PlayerRequirementModelValidator();
            RuleForEach(x => x.Requirements).SetValidator(requirementValidator);
        }
    }
}
