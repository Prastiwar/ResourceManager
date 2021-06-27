using System;

namespace ResourceManager.Core.Validation
{
    public interface IValidationHook
    {
        event EventHandler<ValidatedEventArgs> Validated;
    }
}
