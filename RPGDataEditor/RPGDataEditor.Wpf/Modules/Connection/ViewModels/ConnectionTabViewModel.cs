﻿using FluentValidation.Results;
using Prism.Regions;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Mvvm;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Connection.ViewModels
{
    public class ConnectionTabViewModel : TabViewModel
    {
        public ConnectionTabViewModel(ViewModelContext context) : base(context) { }

        public override Task<bool> CanSwitchTo(NavigationContext navigationContext) => Task.FromResult(true);

        public override async Task<bool> CanSwitchFrom(NavigationContext navigationContext)
        {
            ValidationResult result = await Context.ValidationProvider.ValidateAsync(Session);
            if (result.IsValid)
            {
                bool connected = await Session.ConnectionService.ConnectAsync();
                if(!connected)
                {
                    Context.SnackbarService.Enqueue("Connection lost");
                    return false;
                }
                try
                {
                    Session.SaveSession();
                }
                catch (System.Exception ex)
                {
                    Logger.Error("Couldn't save session", ex);
                }
            }
            return result.IsValid;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext) => await Session.DisconnectAsync();

        public override async Task OnNavigatedFromAsync(NavigationContext navigationContext) => await Session.ConnectAsync();
    }
}
