using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Extensions.Prism.Wpf.ViewModels;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm.Services;

namespace RPGDataEditor.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : PresentableCategoryDataViewModel<Models.Dialogue>
    {
        public DialogueTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<DialogueTabViewModel> logger)
            : base(viewService, dataSource, logger) { }

        protected override PresentableData CreateModelInstance() => new PresentableDialogue() { Category = CurrentCategory };

        protected override Models.Dialogue CreateResource(PresentableData model) => new Models.Dialogue() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };
    }
}
