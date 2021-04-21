using FluentValidation.Results;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Validation;
using System;

namespace RPGDataEditor.Mvvm.Models
{
    public abstract class ObservableModel : BindableClass, ICopiable, IValidable
    {
        public virtual bool CopyValues(object fromCopy)
        {
            try
            {
                this.CopyProperties(fromCopy);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public virtual object DeepClone() => this.DeepClone(GetType());

        public virtual object Clone() => MemberwiseClone();

        public event EventHandler<ValidationResult> Validated;

        public virtual void NotifyValidate(ValidationResult result)
        {
            OnValidated(this, result);
            this.NotifyValidateRecursive(result);
        }

        protected void OnValidated(object sender, ValidationResult result) => Validated?.Invoke(sender, result);
    }
}
