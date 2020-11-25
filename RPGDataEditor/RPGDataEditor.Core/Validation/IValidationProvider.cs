using FluentValidation;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Validation
{
    public interface IValidationProvider
    {
        AbstractValidator<T> CreateAbstractValidator<T>();

        IValidator<T> CreateValidator<T>();

        ValidatorState<T> CreateValidationState<T>(T instance) where T : IValidable;

        Task<ValidationResult> ValidateAsync<T>(T validable) where T : IValidable;
    }
}
