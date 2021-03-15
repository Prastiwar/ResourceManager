﻿using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Dialogue.Views
{
    public partial class DialogueEditor : UserControl
    {
        public DialogueEditor() => InitializeComponent();

        private void ToggleExpandIcon(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Commands.ToggleExpandIcon(btn);
        }

        private void RequirementView_TypeChange(object sender, Wpf.Views.RequirementView.ChangeTypeEventArgs e)
        {
            if (DataContext is ModelDialogViewModel<DialogueModel> vm)
            {
                PlayerRequirementModel newModel = e.CreateRequirement();
                if (newModel != null)
                {
                    int index = vm.Model.Requirements.IndexOf(e.Requirement);
                    if (index > -1)
                    {
                        vm.Model.Requirements.RemoveAt(index);
                        vm.Model.Requirements.Insert(index, newModel);
                    }
                }
            }
        }
    }
}
