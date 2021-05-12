using FluentValidation.Results;
using System;

namespace RPGDataEditor.Core.Validation
{
    public class ValidatedEventArgs : EventArgs
    {
        public ValidatedEventArgs(object instance, ValidationResult result)
        {
            Instance = instance;
            Result = result;
        }

        public object Instance { get; }
        public ValidationResult Result { get; }
    }
}
