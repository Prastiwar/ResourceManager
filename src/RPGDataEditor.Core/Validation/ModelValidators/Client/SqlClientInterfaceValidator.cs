using FluentValidation;
using ResourceManager;
using RPGDataEditor.Core.Connection;

namespace RPGDataEditor.Core.Validation
{
    public class SqlClientInterfaceValidator : AbstractValidator<ISqlClient>
    {
        public SqlClientInterfaceValidator()
        {
            RuleFor(x => (SqlClient)x).SetValidator(new SqlClientValidator()).When(x => x is SqlClient);
        }
    }
}
