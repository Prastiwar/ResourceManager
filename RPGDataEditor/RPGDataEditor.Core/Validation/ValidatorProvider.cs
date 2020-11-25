using FluentValidation;
using FluentValidation.Results;
using Prism.Ioc;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Validation
{
    public class ValidatorProvider : IValidationProvider
    {
        public ValidatorProvider(IContainerProvider container) => this.container = container;

        private readonly IContainerProvider container;

        public IValidator<T> CreateValidator<T>() => container.Resolve<IValidator<T>>();

        public AbstractValidator<T> CreateAbstractValidator<T>() => CreateValidator<T>() as AbstractValidator<T>;

        public ValidatorState<T> CreateValidationState<T>(T validable) where T : IValidable => new ValidatorState<T>(CreateValidator<T>(), validable);

        public Task<ValidationResult> ValidateAsync<T>(T validable) where T : IValidable => CreateValidationState(validable).ValidateNotifyAsync();
    }
}
