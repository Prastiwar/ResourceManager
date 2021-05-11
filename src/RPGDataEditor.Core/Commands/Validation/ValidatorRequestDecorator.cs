using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Commands
{
    public class ValidatorRequestDecorator
    {
        public ValidatorRequestDecorator(IServiceProvider provider) => Provider = provider;

        protected IServiceProvider Provider { get; }

        protected virtual async Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellationToken)
        {
            ValidationContext<object> context = new ValidationContext<object>(instance);
            Type validatorType = typeof(IValidator<>).MakeGenericType(instance.GetType());
            IValidator validator = Provider.GetService(validatorType) as IValidator;
            return validator != null ? await validator.ValidateAsync(context) : new ValidationResult();
        }
    }
}
