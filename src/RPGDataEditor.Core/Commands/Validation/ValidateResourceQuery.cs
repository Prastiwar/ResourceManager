using FluentValidation.Results;
using ResourceManager.Commands;
using System;

namespace RPGDataEditor.Core.Commands
{
    public sealed class ValidateResourceQuery : ResourceRequest<object, ValidationResult>
    {
        public ValidateResourceQuery(object resource) : this(resource, resource.GetType()) { }
        public ValidateResourceQuery(object resource, Type resourceType) : base(resource) => ResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));

        public Type ResourceType { get; }
    }
}
