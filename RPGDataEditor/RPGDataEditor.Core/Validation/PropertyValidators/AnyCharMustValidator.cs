using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGDataEditor.Core.Validation
{
    public class AnyCharMustValidator : PropertyValidator
    {
        private readonly Func<char, bool> charFunc;

        public AnyCharMustValidator(Func<char, bool> charFunc) : base(nameof(AnyCharMustValidator)) => this.charFunc = charFunc;

        protected override bool IsValid(PropertyValidatorContext context) => context.PropertyValue is IEnumerable<char> text && text.Any(charFunc);
    }
}
