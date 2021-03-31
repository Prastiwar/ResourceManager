﻿using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueEditorViewModel : ModelDialogViewModel<DialogueModel>
    {
        public DialogueEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Dialogue Editor";

        public ICommand AddOptionCommand => Commands.AddListItemCommand(() => Model.Options);
    }
}
