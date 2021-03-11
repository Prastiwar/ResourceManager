using FluentValidation;
using RPGDataEditor.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace RPGDataEditor.Core.Validation
{
    public class QuestModelValidator : AbstractValidator<QuestModel>
    {
        public QuestModelValidator()
        {
            RuleFor(x => x.CompletionTask).NotEmpty().WithMessage("Quest needs task that will complete quest");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title cannot be empty");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message should not be empty");

            RuleFor(x => x.CompletionTask).Must(ValidTask)
                                          .WithMessage("This is not valid task");

            RuleFor(x => x.Tasks).Must(ValidTasks).WithMessage("At least one Task is not valid");
        }

        private bool ValidTasks(IList<QuestTask> tasks) => tasks == null || !tasks.Any(task => !ValidTask(task));

        private bool ValidTask(QuestTask task)
        {
            if (task is KillQuestTask killTask)
            {
                return ValidationExtensions.IsResourceLocation(killTask.Kill, false);
            }
            else if (task is RightItemInteractQuestTask itemTask)
            {
                return ValidationExtensions.IsResourceLocation(itemTask.Item, false) && ValidationExtensions.IsJson(itemTask.Nbt);
            }
            return true;
        }
    }
}
