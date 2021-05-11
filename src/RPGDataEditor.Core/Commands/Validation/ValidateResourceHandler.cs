using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Commands
{
    public class ValidateResourceHandler : ValidatorRequestDecorator, IRequestHandler<ValidateResourceQuery, ValidationResult>
    {
        public ValidateResourceHandler(IServiceProvider provider) : base(provider) { }

        public Task<ValidationResult> Handle(ValidateResourceQuery request, CancellationToken cancellationToken) => ValidateAsync(request.Resource, cancellationToken);
    }
}
