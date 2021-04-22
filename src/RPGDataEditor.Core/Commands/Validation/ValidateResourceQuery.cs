using FluentValidation.Results;
using ResourceManager.Commands;

namespace RPGDataEditor.Core.Commands
{
    public class ValidateResourceQuery<TResource> : ResourceQuery<TResource, ValidationResult>
    {
        public ValidateResourceQuery(TResource resource) : base(resource) { }
    }
}
