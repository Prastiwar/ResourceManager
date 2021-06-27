using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using System.Collections.Generic;

namespace ResourceManager.Core.Validation
{
    /// <summary>
    /// Holds IsPropertyValid that returns true when all previous validators succeded 
    /// Remember to keep instance to get its value
    /// </summary>
    public class CheckValidValidator : PropertyValidator
    {
        public CheckValidValidator() : base() { }

        public bool IsPropertyValid { get; protected set; }

        private void Invalid(object o, IEnumerable<ValidationFailure> failures) => IsPropertyValid = false;

        protected override bool IsValid(PropertyValidatorContext context)
        {
            context.Rule.CascadeMode = CascadeMode.Stop; // to prevent calling this method when property is not valid
            context.Rule.OnFailure -= Invalid; // make sure not to duplicate event
            context.Rule.OnFailure += Invalid;
            IsPropertyValid = true;
            return IsPropertyValid;
        }
    }
}
