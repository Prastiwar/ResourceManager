using FluentValidation.Results;
using ResourceManager.Commands;
using System;

namespace RPGDataEditor.Core.Commands
{
    public sealed class ValidateResourceQuery : ResourceRequest<ValidationResult>
    {
        public ValidateResourceQuery(object resource) : this(resource.GetType(), resource) { }

        public ValidateResourceQuery(Type resourceType, object resource) : base(resourceType, resource) { }
    }
}
