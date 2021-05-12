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

        protected virtual async Task<ValidationResult> ValidateAsync(object instance, Type instanceType = default, CancellationToken cancellationToken = default)
        {
            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (instanceType is null)
            {
                instanceType = instance.GetType();
            }
            if (!instanceType.IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException($"{nameof(instanceType)} is not assignable from {instance}");
            }
            ValidationContext<object> context = new ValidationContext<object>(instance);
            Type validatorType = typeof(IValidator<>).MakeGenericType(instanceType);
            try
            {
                IValidator validator = Provider.GetService(validatorType) as IValidator;
                return await validator.ValidateAsync(context);
            }
            catch (ValidationException ex)
            {
                return new ValidationResult(ex.Errors);
            }
            catch (Exception ex)
            {
                return new ValidationResult();
            }
        }
    }
}
