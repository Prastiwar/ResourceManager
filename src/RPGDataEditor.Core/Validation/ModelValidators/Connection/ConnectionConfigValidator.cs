using FluentValidation;
using RPGDataEditor.Connection;
using System.IO;
using System.Security;

namespace RPGDataEditor.Core.Validation
{
    public class ConnectionConfigValidator : AbstractValidator<IConnectionConfig>
    {
        public ConnectionConfigValidator()
        {
            RuleFor(x => x.Get(nameof(ConnectionSettings.Type))).NotNull().WithMessage($"{nameof(ConnectionSettings.Type)} must be provided");

            RuleFor(x => x.Get("ConnectionString")).NotEmpty().WithMessage(CustomMessages.Empty)
                                       .When(x => IsType(x, "sql"));

            RuleFor(x => x.Get("FolderPath")).Must(x => Directory.Exists(x.ToString())).WithMessage("Folder doesn't exists")
                                       .When(x => IsType(x, "local"));

            RuleFor(x => x.Get("Host")).NotEmpty().WithMessage(CustomMessages.Empty)
                                       .When(x => IsType(x, "ftp"));
            RuleFor(x => x.Get("UserName")).NotEmpty().WithMessage(CustomMessages.Empty)
                                       .When(x => IsType(x, "ftp"));
            RuleFor(x => (x.Get("Password") as SecureString).Length).GreaterThan(0).WithMessage(CustomMessages.Empty)
                                                                    .When(x => IsType(x, "ftp") && x.Get("Password") is SecureString);
            RuleFor(x => (x.Get("Password") as string).Length).GreaterThan(0).WithMessage(CustomMessages.Empty)
                                                                    .When(x => IsType(x, "ftp") && x.Get("Password") is string);

            static bool IsType(IConnectionConfig x, string type) => string.Compare(x.Get(nameof(ConnectionSettings.Type)).ToString(), type, true) == 0;
        }
    }
}
