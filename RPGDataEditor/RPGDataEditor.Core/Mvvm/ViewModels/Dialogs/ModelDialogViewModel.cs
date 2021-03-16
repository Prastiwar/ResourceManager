using Prism.Services.Dialogs;
using RPGDataEditor.Core.Validation;
using System;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class ModelDialogViewModel<TModel> : DialogViewModelBase where TModel : IValidable
    {
        public ModelDialogViewModel(ViewModelContext context) : base(context) { }

        private TModel model;
        public TModel Model {
            get => model;
            set => SetProperty(ref model, value);
        }

        protected sealed override void CloseDialog(object result) => Close(result is bool b && b);

        protected virtual async void Close(bool result)
        {
            bool isCancelled = await ShouldCancelAsync(result).ConfigureAwait(true);
            if (!isCancelled)
            {
                await OnDialogClosing(result).ConfigureAwait(true);
                Close(new ModelDialogParameters<TModel>(Model) { IsSuccess = result }.Build());
            }
        }

        public virtual Task OnDialogClosing(bool result) => Task.FromResult(true);

        protected virtual async Task<bool> ShouldCancelAsync(bool result)
        {
            if (result)
            {
                FluentValidation.Results.ValidationResult validationResult = await Context.ValidationProvider.ValidateAsync(Model);
                return !validationResult.IsValid;
            }
            return false;
        }

        protected override Task InitializeAsync(IDialogParameters parameters)
        {
            string keyName = nameof(ModelDialogParameters<TModel>.Model);
            Model = parameters.GetValue<TModel>(keyName) ?? throw new ArgumentNullException(keyName, "Cannot show model dialog when model is not passed");
            return Task.CompletedTask;
        }
    }
}
