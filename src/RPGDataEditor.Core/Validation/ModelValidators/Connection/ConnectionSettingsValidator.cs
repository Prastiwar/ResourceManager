using FluentValidation;
using RPGDataEditor.Connection;

namespace RPGDataEditor.Core.Validation
{
    public class ConnectionSettingsValidator : AbstractValidator<IConnectionSettings>
    {
        public ConnectionSettingsValidator() => RuleFor(x => x.CreateConfig()).SetValidator(new ConnectionConfigValidator());
    }
}
