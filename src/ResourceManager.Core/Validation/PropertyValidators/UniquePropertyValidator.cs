﻿using FluentValidation.Validators;
using System.Collections.Generic;
using System.Linq;

namespace ResourceManager.Core.Validation
{
    public class UniquePropertyValidator<T, TProperty> : PropertyValidator
    {
        public UniquePropertyValidator(IEnumerable<T> repository) : base() => Repository = repository;

        public IEnumerable<T> Repository { get; }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            TProperty propertyValue = context.PropertyValue is TProperty prop ? prop : default;
            if (propertyValue == null)
            {
                return true;
            }
            bool isUnique = !Repository.Any(x => {
                object value = x.GetType().GetProperty(context.PropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetValue(x);
                if (value is TProperty val)
                {
                    bool isEqual = val.Equals(propertyValue);
                    return isEqual;
                }
                return false;
            });
            return isUnique;
        }

        protected override string GetDefaultMessageTemplate() => nameof(UniquePropertyValidator<T, TProperty>);
    }
}
