using FluentValidation;
using RPGDataEditor.Core.Connection;
using RPGDataEditor.Core.Services;
using System.IO;

namespace RPGDataEditor.Core.Validation
{
    public class ConnectionValidator : AbstractValidator<IConnectionService>
    {
        public ConnectionValidator()
        {
            RuleFor(x => ((ExplorerController)x).FolderPath).Must(path => Directory.Exists(path))
                                                            .WithMessage("Folder doesn't exists")
                                                            .When(x => x is ExplorerController);

            RuleFor(x => ((FtpController)x).Host).NotEmpty().WithMessage("Host cannot be empty").When(x => x is FtpController);
            RuleFor(x => ((FtpController)x).UserName).NotEmpty().WithMessage("Username cannot be empty").When(x => x is FtpController);
            RuleFor(x => ((FtpController)x).Password).NotEmpty().WithMessage("Password cannot be empty").When(x => x is FtpController);
        }
    }
}
