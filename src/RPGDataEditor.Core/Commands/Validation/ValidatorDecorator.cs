using FluentValidation;
using FluentValidation.Results;
using ResourceManager;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Commands
{
    public class ValidatorDecorator<TValidable>
    {
        public ValidatorDecorator(IEnumerable<IValidator<TValidable>> validators) => Validators = validators;

        protected IEnumerable<IValidator<TValidable>> Validators { get; }

        protected bool StopOnFirstFail { get; set; }

        protected virtual async Task<ValidationResult> ValidateAsync(TValidable request, CancellationToken cancellationToken)
        {
            IList<ValidationFailure> failures = new List<ValidationFailure>();

            foreach (IValidator<TValidable> validator in Validators)
            {
                ValidationResult results = await validator.ValidateAsync(request);
                if (StopOnFirstFail && !results.IsValid)
                {
                    throw new ValidationException(results.Errors);
                }
                failures.AddRange(results.Errors);
            }
            return new ValidationResult(failures.Where(failure => failure != null));
        }
    }
}
