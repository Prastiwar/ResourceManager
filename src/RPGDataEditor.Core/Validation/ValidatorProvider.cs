using FluentValidation;
using FluentValidation.Results;
using RPGDataEditor.Services;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Validation
{
    public sealed class ValidatorProvider : IValidationProvider
    {
        public ValidatorProvider(IResolverService resolver) => this.resolver = resolver;

        private readonly IResolverService resolver;

        public IValidator<T> CreateValidator<T>() => resolver.Resolve<IValidator<T>>();

        public AbstractValidator<T> CreateAbstractValidator<T>() => CreateValidator<T>() as AbstractValidator<T>;

        public ValidatorState<T> CreateValidationState<T>(T validable) where T : IValidable => new ValidatorState<T>(CreateValidator<T>(), validable);

        public Task<ValidationResult> ValidateAsync<T>(T validable) where T : IValidable => CreateValidationState(validable).ValidateNotifyAsync();
    }
}
