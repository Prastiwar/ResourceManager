using FluentValidation.Results;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Validation;
using System;
using System.Reflection;

namespace RPGDataEditor.Core.Models
{
    public abstract class ObservableModel : BindableClass, ICopiable, IValidable
    {
        public virtual bool CopyValues(object fromCopy) => CopyProperties(fromCopy, (prop) => {
            PropertyInfo fromCopyProp = fromCopy.GetType().GetProperty(prop.Name);
            bool canSetValue = prop.SetMethod != null && fromCopyProp != null && fromCopyProp.PropertyType == prop.PropertyType;
            if (canSetValue)
            {
                object value = fromCopyProp.GetValue(fromCopy);
                prop.SetValue(this, value);
            }
        });

        private bool CopyProperties(object fromCopy, Action<PropertyInfo> copyProperty)
        {
            try
            {
                bool hasCopyRelatioship = GetType().IsAssignableFrom(fromCopy.GetType()) || fromCopy.GetType().IsAssignableFrom(GetType());
                if (!hasCopyRelatioship)
                {
                    return false;
                }
                PropertyInfo[] props = GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    copyProperty(prop);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public virtual object DeepClone() => DeepCopy.DeepCopier.Copy(this, GetType());

        public virtual object Clone() => MemberwiseClone();

        public event EventHandler<ValidationResult> OnValidated;

        public void NotifyValidate(ValidationResult result) => OnValidated?.Invoke(this, result);
    }
}
