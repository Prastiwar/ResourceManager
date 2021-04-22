using FluentValidation;
using RPGDataEditor.Core.Connection;

namespace RPGDataEditor.Core.Validation
{
    public class SqlClientValidator : AbstractValidator<SqlClient>
    {
        public SqlClientValidator() => RuleFor(x => x.ConnectionString).NotEmpty().WithMessage(CustomMessages.Empty);
    }
}
