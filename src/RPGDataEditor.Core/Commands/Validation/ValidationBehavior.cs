using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Commands
{
    public class ValidationBehavior<TRequest, TResponse> : ValidatorDecorator<TRequest>, IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) : base(validators) { }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            ValidationResult results = await ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }
            return await next();
        }
    }
}
