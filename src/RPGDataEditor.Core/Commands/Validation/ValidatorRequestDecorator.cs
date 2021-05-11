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
            try
            {
                IValidator validator = Provider.GetService(validatorType) as IValidator;
                return await validator.ValidateAsync(context);
            }
            catch (ValidationException ex)
            {
                return new ValidationResult(ex.Errors);
            }
            catch (Exception)
            {
                return new ValidationResult();
            }
        }
    }
}
