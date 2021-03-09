using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class DialogueModelValidator : AbstractValidator<DialogueModel>
    {
        public DialogueModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title cannot be empty");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message should not be empty");
        }
    }
}
