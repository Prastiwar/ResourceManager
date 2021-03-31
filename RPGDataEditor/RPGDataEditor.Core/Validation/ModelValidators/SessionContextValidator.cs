using FluentValidation;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Core.Validation
{
    public class SessionContextValidator : AbstractValidator<ISessionContext>
    {
        public SessionContextValidator()
        {
            RuleFor(x => x.Client).SetValidator(new ConnectionValidator());
            RuleFor(x => ((DefaultSessionContext)x).Options).SetValidator(new OptionsDataValidator()).When(x => x is DefaultSessionContext);
        }
    }
}
