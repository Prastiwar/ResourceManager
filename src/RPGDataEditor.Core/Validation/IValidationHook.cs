using System;

namespace RPGDataEditor.Core.Validation
{
    public interface IValidationHook
    {
        event EventHandler<ValidatedEventArgs> Validated;
    }
}
