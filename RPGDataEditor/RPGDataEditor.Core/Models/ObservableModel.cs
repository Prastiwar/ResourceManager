using FluentValidation.Results;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGDataEditor.Core.Models
{
    public abstract class ObservableModel : BindableClass, ICopiable, IValidable
    {
        public virtual bool CopyValues(object fromCopy) => CopyProperties(fromCopy, (prop) => {
            PropertyInfo fromCopyProp = fromCopy.GetType().GetProperty(prop.Name);
            bool canSetValue = prop.SetMethod != null && fromCopyProp != null && fromCopyProp.PropertyType == prop.PropertyType;
            bool canGetValue = fromCopyProp.GetMethod != null;
            if (canSetValue && canGetValue)
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

        public event EventHandler<ValidationResult> Validated;

        public virtual void NotifyValidate(ValidationResult result)
        {
            OnValidated(this, result);
            HashSet<string> notifiedPropertyNames = new HashSet<string>();
            List<ValidationFailure> newFailures = new List<ValidationFailure>();
            foreach (ValidationFailure failure in result.Errors)
            {
                string[] propertyNames = failure.PropertyName.Split('.', StringSplitOptions.RemoveEmptyEntries);
                if (propertyNames.Length < 2)
                {
                    continue;
                }
                string targetPropertyName = GetNoArrayProperty(propertyNames[0]);
                if (!notifiedPropertyNames.Add(targetPropertyName))
                {
                    continue;
                }
                PropertyInfo property = GetType().GetProperty(targetPropertyName);
                bool cannotValidate = property.GetMethod == null || property.GetCustomAttribute<NotValidableAttribute>() != null;
                if (cannotValidate)
                {
                    continue;
                }
                object value = property.GetValue(this);
                if (value is IValidable validable)
                {
                    ValidationResult newResult = GetTargetPropertyResult(result, newFailures, targetPropertyName);
                    validable.NotifyValidate(newResult);
                }
                else if (value is IEnumerable enumerable && !(enumerable is IEnumerable<char>))
                {
                    ValidationResult collectionResults = GetTargetPropertyResult(result, newFailures, targetPropertyName);
                    List<object> enumerableItems = enumerable.Cast<object>().ToList();
                    foreach (ValidationFailure item in collectionResults.Errors)
                    {
                        int arrayEndIndex = item.PropertyName.IndexOf(']');
                        string itemIndexString = item.PropertyName.Substring(1, arrayEndIndex - 1);
                        int itemIndex = int.Parse(itemIndexString);
                        if (enumerableItems[itemIndex] is IValidable itemValidable)
                        {
                            string targetItemPropertyName = item.PropertyName.Substring(0, item.PropertyName.IndexOf('.'));
                            ValidationResult itemResults = GetTargetPropertyResult(collectionResults, newFailures, targetItemPropertyName);
                            itemValidable.NotifyValidate(itemResults);
                        }
                    }
                }
            }
        }

        private string GetNoArrayProperty(string propertyName)
        {
            int startArrayIndex = propertyName.IndexOf('[');
            if (startArrayIndex > -1)
            {
                return propertyName.Substring(0, startArrayIndex);
            }
            return propertyName;
        }

        private ValidationResult GetTargetPropertyResult(ValidationResult result, List<ValidationFailure> newFailures, string targetPropertyName)
        {
            newFailures.Clear();

            bool IsTargetFailure(ValidationFailure failure)
            {
                bool starts = failure.PropertyName.StartsWith(targetPropertyName);
                if (starts && failure.PropertyName.Length > targetPropertyName.Length)
                {
                    char nextChar = failure.PropertyName[targetPropertyName.Length];
                    return nextChar == '[' || nextChar == '.';
                }
                return starts;
            }

            IEnumerable<ValidationFailure> segmentFailures = result.Errors.Where(IsTargetFailure);
            foreach (ValidationFailure segmentFailure in segmentFailures)
            {
                bool isStartArray = segmentFailure.PropertyName[targetPropertyName.Length] == '[';
                int removeCount = isStartArray ? targetPropertyName.Length : targetPropertyName.Length + 1;
                string trimmedPropertyName = segmentFailure.PropertyName.Remove(0, removeCount);
                ValidationFailure newFailure = segmentFailure.Copy();
                newFailure.PropertyName = trimmedPropertyName;
                newFailures.Add(newFailure);
            }
            return new ValidationResult(newFailures);
        }

        protected void OnValidated(object sender, ValidationResult result) => Validated?.Invoke(sender, result);
    }
}
