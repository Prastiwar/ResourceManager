﻿using FluentValidation;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Core.Validation
{
    public class SessionContextValidator : AbstractValidator<SessionContext>
    {
        public SessionContextValidator()
        {
            RuleFor(x => x.Client).SetValidator(new ConnectionValidator());
            RuleFor(x => x.Options).SetValidator(new OptionsDataValidator());
        }
    }
}
