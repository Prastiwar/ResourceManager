using FluentValidation;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Validation
{
    public class OptionsDataValidator : AbstractValidator<OptionsData>
    {
        public OptionsDataValidator()
        {
            string backupEmpty = "Backup path cannot be empty when BackupOnSaving is enabled";
            RuleFor(x => x.NpcBackupPath).NotEmpty().WithMessage(backupEmpty).When(x => x.BackupOnSaving);
            RuleFor(x => x.DialoguesBackupPath).NotEmpty().WithMessage(backupEmpty).When(x => x.BackupOnSaving);
            RuleFor(x => x.QuestsBackupPath).NotEmpty().WithMessage(backupEmpty).When(x => x.BackupOnSaving);
        }
    }
}
