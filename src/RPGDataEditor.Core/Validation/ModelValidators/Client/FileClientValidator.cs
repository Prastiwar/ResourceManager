using FluentValidation;
using ResourceManager;
using RPGDataEditor.Core.Connection;

namespace RPGDataEditor.Core.Validation
{
    public class FileClientValidator : AbstractValidator<IFileClient>
    {
        public FileClientValidator()
        {
            RuleFor(x => (LocalFileClient)x).SetValidator(new LocalFileClientValidator()).When(x => x is LocalFileClient);
            RuleFor(x => (FtpFileClient)x).SetValidator(new FtpFileClientValidator()).When(x => x is FtpFileClient);
        }
    }
}
