using FluentValidation.Results;
using ResourceManager.Commands;

namespace RPGDataEditor.Core.Commands
{
    public class ValidateResourceQuery : ResourceRequest<object, ValidationResult>
    {
        public ValidateResourceQuery(object resource) : base(resource) { }
    }
}
