﻿using MediatR;
using Microsoft.Extensions.Logging;
using Prism.Services.Dialogs;
using RPGDataEditor.Core.Commands;
using RPGDataEditor.Extensions.Prism.Wpf;
using System;
using System.Threading.Tasks;

namespace RPGDataEditor.Mvvm
{
    public abstract class ModelDialogViewModel<TModel> : DialogViewModelBase
    {
        public ModelDialogViewModel(IMediator mediator, ILogger<ModelDialogViewModel<TModel>> logger) : base(logger) => Mediator = mediator;

        private TModel model;
        protected TModel Model {
            get => model;
            set => SetProperty(ref model, value);
        }

        protected IMediator Mediator { get; }

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
                FluentValidation.Results.ValidationResult validationResult = await Mediator.Send(new ValidateResourceQuery(Model));
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
