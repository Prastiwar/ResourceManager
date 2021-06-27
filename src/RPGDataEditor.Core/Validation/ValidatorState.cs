using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Validation
{
    public class ValidatorState<T> : AbstractValidator<T> where T : IValidable
    {
        public ValidatorState(IValidator<T> validator, T instance)
        {
            this.validator = validator;
            this.instance = instance;
        }

        private readonly IValidator<T> validator;
        private readonly T instance;

        public ValidationStatus Status { get; protected set; }

        protected IDictionary<string, IList<string>> CustomErrors { get; } = new Dictionary<string, IList<string>>();

        public void AddErrors(IEnumerable<KeyValuePair<string, string[]>> errors)
        {
            foreach (KeyValuePair<string, string[]> pair in errors)
            {
                foreach (string error in pair.Value)
                {
                    AddError(pair.Key, error);
                }
            }
        }

        public void AddError(string propertyName, Exception exception)
        {
            AddError(propertyName, exception.Message);
            Status = ValidationStatus.Unvalidated;
        }

        public void AddError(string propertyName, string message)
        {
            IList<string> list = GetErrorList(propertyName);
            list.Add(message);
            Status = ValidationStatus.Unvalidated;
        }

        public void ClearState()
        {
            CustomErrors.Clear();
            Status = ValidationStatus.Unvalidated;
        }

        public void ClearState(string propertyName)
        {
            GetErrorList(propertyName).Clear();
            Status = ValidationStatus.Unvalidated;
        }

        /// <summary> Validates then notifies handlers about validation </summary>
        public async Task<ValidationResult> ValidateNotifyAsync()
        {
            ValidationResult result = await validator.ValidateAsync(instance);
            foreach (KeyValuePair<string, IList<string>> keyValue in CustomErrors)
            {
                foreach (string error in keyValue.Value)
                {
                    result.Errors.Add(new ValidationFailure(keyValue.Key, error));
                }
            }
            Status = result.IsValid ? ValidationStatus.Valid : ValidationStatus.Invalid;
            instance.NotifyValidate(result);
            return result;
        }

        private IList<string> GetErrorList(string propertyName)
        {
            if (CustomErrors.TryGetValue(propertyName, out IList<string> list))
            {
                return list;
            }
            return CustomErrors[propertyName] = new List<string>();
        }
    }
}
