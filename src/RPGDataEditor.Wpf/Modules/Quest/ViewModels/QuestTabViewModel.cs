using Microsoft.Extensions.Logging;
using Prism.Commands;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;
using RPGDataEditor.Mvvm.Services;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestTabViewModel : PresentableCategoryDataViewModel<Models.Quest>
    {
        public QuestTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<QuestTabViewModel> logger)
            : base(viewService, dataSource, logger) { }

        public ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<PresentableData>(async presentable => await OpenEditorAsync(presentable));

        public ICommand removeModelCommand;
        public ICommand RemoveModelCommand => removeModelCommand ??= new DelegateCommand<PresentableData>(async presentable => await RemoveModelAsync(presentable));

        protected override PresentableData CreateModelInstance() => new PresentableQuest() { Category = CurrentCategory };

        protected override Models.Quest CreateResource(PresentableData model) => new Models.Quest() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };
    }
}
