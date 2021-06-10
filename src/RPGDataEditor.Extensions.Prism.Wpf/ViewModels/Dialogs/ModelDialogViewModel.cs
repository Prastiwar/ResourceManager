using FluentValidation;
using Microsoft.Extensions.Logging;
using Prism.Services.Dialogs;
using RPGDataEditor.Extensions.Prism.Wpf;
using System;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public abstract class ModelDialogViewModel<TModel> : DialogViewModelBase
    {
        public ModelDialogViewModel(IValidator<TModel> validator, ILogger<ModelDialogViewModel<TModel>> logger) : base(logger) => Validator = validator;

        private TModel model;
        public TModel Model {
            get => model;
            set => SetProperty(ref model, value);
        }

        protected IValidator<TModel> Validator { get; }

        public override string Title => typeof(TModel).Name + " Editor";

        protected sealed override void CloseDialog(object result) => Close(result is bool b && b);

        protected virtual async void Close(bool result)
        {
            bool isCancelled = await ShouldCancelAsync(result).ConfigureAwait(true);
            if (!isCancelled)
            {
                await OnDialogClosing(result).ConfigureAwait(true);
                Close(new Navigation.ModelDialogParameters<TModel>(Model) { IsSuccess = result }.BuildPrism());
            }
        }

        public virtual Task OnDialogClosing(bool result) => Task.FromResult(true);

        protected virtual async Task<bool> ShouldCancelAsync(bool result)
        {
            if (result)
            {
                FluentValidation.Results.ValidationResult validationResult = await Validator.ValidateAsync(Model);
                return !validationResult.IsValid;
            }
            return false;
        }

        protected override Task InitializeAsync(IDialogParameters parameters)
        {
            string keyName = nameof(Navigation.ModelDialogParameters<TModel>.Model);
            Model = parameters.GetValue<TModel>(keyName) ?? throw new ArgumentNullException(keyName, "Cannot show model dialog when model is not passed");
            return Task.CompletedTask;
        }
    }
}
