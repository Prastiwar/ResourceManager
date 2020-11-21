using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Validation;
using FluentValidation.Results;
using System;
using System.Reflection;

namespace RPGDataEditor.Core.Models
{
    public abstract class ObservableModel : BindableClass, ICopiable, IValidable
    {
        public virtual bool CopyValues(object fromCopy)
        {
            try
            {
                bool hasRelationship = GetType().IsAssignableFrom(fromCopy.GetType()) || fromCopy.GetType().IsAssignableFrom(GetType());
                if (!hasRelationship)
                {
                    return false;
                }
                PropertyInfo[] props = GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    PropertyInfo fromCopyProp = fromCopy.GetType().GetProperty(prop.Name);
                    bool canSetValue = prop.SetMethod != null && fromCopyProp != null && fromCopyProp.PropertyType == prop.PropertyType;
                    if (canSetValue)
                    {
                        object value = fromCopyProp.GetValue(fromCopy);
                        prop.SetValue(this, value);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public virtual object DeepClone()
        {
            ObservableModel model = (ObservableModel)Activator.CreateInstance(GetType(), true);
            model.CopyValues(this);
            return model;
        }

        public virtual object Clone() => MemberwiseClone();

        public event EventHandler<ValidationResult> OnValidated;

        public void NotifyValidate(ValidationResult result) => OnValidated?.Invoke(this, result);
    }
}
