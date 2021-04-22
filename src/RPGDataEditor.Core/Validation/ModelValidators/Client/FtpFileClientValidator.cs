using FluentValidation;
using RPGDataEditor.Core.Connection;

namespace RPGDataEditor.Core.Validation
{
    public class FtpFileClientValidator : AbstractValidator<FtpFileClient>
    {
        public FtpFileClientValidator()
        {
            RuleFor(x => x.Host).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.UserName).NotEmpty().WithMessage(CustomMessages.Empty);
            RuleFor(x => x.Password.Length).GreaterThan(0).WithMessage(CustomMessages.Empty);
        }
    }
}