using FluentValidation;
using RPGDataEditor.Core.Mvvm;
using System.IO;

namespace RPGDataEditor.Core.Validation
{
    public class SessionContextValidator : AbstractValidator<SessionContext>
    {
        public SessionContextValidator()
        {
            string locationNotExists = "Location doesn't exists";
            RuleFor(x => x.LocationPath).NotEmpty().WithMessage(locationNotExists);

            RuleFor(x => x.LocationPath).Must(path => Directory.Exists(path)).WithMessage(locationNotExists).When(x => !x.IsFtp);

            RuleFor(x => x.FtpUserName).NotEmpty().WithMessage("Username cannot be empty").When(context => context.IsFtp);
            RuleFor(x => x.FtpPassword).NotEmpty().WithMessage("Password cannot be empty").When(context => context.IsFtp);

            RuleFor(x => x.Options.NpcBackupPath).NotEmpty().WithMessage("Backup path cannot be empty when BackupOnSaving is enabled").When(x => x.Options.BackupOnSaving);
            RuleFor(x => x.Options.DialoguesBackupPath).NotEmpty().WithMessage("Backup path cannot be empty when BackupOnSaving is enabled").When(x => x.Options.BackupOnSaving);
            RuleFor(x => x.Options.QuestsBackupPath).NotEmpty().WithMessage("Backup path cannot be empty when BackupOnSaving is enabled").When(x => x.Options.BackupOnSaving);
        }
    }
}
