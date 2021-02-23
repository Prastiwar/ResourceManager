using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public abstract class ModelDialogViewModel<TModel> : DialogViewModelBase
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

        protected virtual Task<bool> ShouldCancelAsync(bool result) => Task.FromResult(false);

        protected override Task InitializeAsync(IDialogParameters parameters)
        {
            string keyName = nameof(ModelDialogParameters<TModel>.Model);
            Model = parameters.GetValue<TModel>(keyName) ?? throw new ArgumentNullException(keyName, "Cannot show model dialog when model is not passed");
            return Task.CompletedTask;
        }
    }
}
