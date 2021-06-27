using System;
using FluentValidation.Results;

namespace RPGDataEditor.Core.Validation
{
    public interface IValidable
    {
        void NotifyValidate(ValidationResult result);

        event EventHandler<ValidationResult> Validated;
    }
}
