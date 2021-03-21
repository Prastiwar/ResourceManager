using FluentValidation;
using RPGDataEditor.Core.Connection;
using System.IO;

namespace RPGDataEditor.Core.Validation
{
    public class ConnectionValidator : AbstractValidator<IResourceClient>
    {
        public ConnectionValidator()
        {
            RuleFor(x => ((ExplorerResourceClient)x).FolderPath).Must(path => Directory.Exists(path))
                                                            .WithMessage("Folder doesn't exists")
                                                            .When(x => x is ExplorerResourceClient);

            RuleFor(x => ((FtpResourceClient)x).Host).NotEmpty().WithMessage("Host cannot be empty").When(x => x is FtpResourceClient);
            RuleFor(x => ((FtpResourceClient)x).UserName).NotEmpty().WithMessage("Username cannot be empty").When(x => x is FtpResourceClient);
            RuleFor(x => ((FtpResourceClient)x).Password).NotEmpty().WithMessage("Password cannot be empty").When(x => x is FtpResourceClient);
        }
    }
}
