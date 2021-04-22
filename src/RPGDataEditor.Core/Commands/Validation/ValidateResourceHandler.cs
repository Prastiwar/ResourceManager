using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Commands
{
    public class ValidateResourceHandler<TResource> : ValidatorDecorator<TResource>, IRequestHandler<ValidateResourceQuery<TResource>, ValidationResult>
    {
        public ValidateResourceHandler(IEnumerable<IValidator<TResource>> validators) : base(validators) { }

        public Task<ValidationResult> Handle(ValidateResourceQuery<TResource> request, CancellationToken cancellationToken) => ValidateAsync(request.Resource, cancellationToken);
    }
}
