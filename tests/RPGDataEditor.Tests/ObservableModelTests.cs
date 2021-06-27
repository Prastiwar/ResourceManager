using FluentValidation.Results;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Validation;
using System;
using System.Collections.Generic;
using Xunit;

namespace RPGDataEditor.Tests
{
    public class ObservableModelTests
    {
        public class TestValidable : IValidable
        {
            public string TestProperty { get; set; }

            public event EventHandler<ValidationResult> Validated;

            public void NotifyValidate(ValidationResult result)
            {
                Assert.False(result.IsValid);
                Assert.NotEmpty(result.Errors);
                Assert.Equal(1, result.Errors.Count);
                Assert.Equal(nameof(TestProperty), result.Errors[0].PropertyName);
            }
        }

        public class TestObservableModel : IValidable
        {
            public TestObservableModel()
            {
                Validable = new TestValidable();
                Validables = new List<TestValidable>() {
                    new TestValidable(),
                    new TestValidable()
                };
            }

            public int TestProperty { get; }
            public TestValidable Validable { get; }
            public IList<TestValidable> Validables { get; }

            public event EventHandler<ValidationResult> Validated;

            public void NotifyValidate(ValidationResult result) => this.NotifyValidateRecursive(result);
        }

        [Fact]
        public void ModelNotifyValidation()
        {
            TestObservableModel model = new TestObservableModel();
            List<ValidationFailure> failures = new List<ValidationFailure>() {
                new ValidationFailure(nameof(TestObservableModel.TestProperty), "Error Message"),
                new ValidationFailure($"{nameof(TestObservableModel.Validable)}.{nameof(TestValidable.TestProperty)}", "Error Message")
            };
            for (int i = 0; i < model.Validables.Count; i++)
            {
                failures.Add(new ValidationFailure($"{nameof(TestObservableModel.Validables)}[{i}].{nameof(TestValidable.TestProperty)}", "Error Message"));
            }
            ValidationResult result = new ValidationResult(failures);
            Assert.False(result.IsValid);
            model.NotifyValidate(result);
        }
    }
}
