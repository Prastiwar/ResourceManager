using Microsoft.Extensions.Logging;
using Prism.Commands;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Mvvm.Services;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : PresentableCategoryDataViewModel<Models.Dialogue>
    {
        public DialogueTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<DialogueTabViewModel> logger)
            : base(viewService, dataSource, logger) { }

        public ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<PresentableData>(async presentable => await OpenEditorAsync(presentable));

        public ICommand removeModelCommand;
        public ICommand RemoveModelCommand => removeModelCommand ??= new DelegateCommand<PresentableData>(async presentable => await RemoveModelAsync(presentable));

        protected override PresentableData CreateModelInstance() => new PresentableDialogue() { Category = CurrentCategory };

        protected override Models.Dialogue CreateResource(PresentableData model) => new Models.Dialogue() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };
    }
}
