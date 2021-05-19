﻿using MediatR;
using Microsoft.Extensions.Logging;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueEditorViewModel : ModelDialogViewModel<Models.Dialogue>
    {
        public DialogueEditorViewModel(IMediator mediator, ILogger<DialogueEditorViewModel> logger) : base(mediator, logger) { }

        public override string Title => "Dialogue Editor";
    }
}
