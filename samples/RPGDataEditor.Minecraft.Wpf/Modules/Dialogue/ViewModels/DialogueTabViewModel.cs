using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Mvvm.Services;

namespace RPGDataEditor.Minecraft.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : RPGDataEditor.Wpf.Dialogue.ViewModels.DialogueTabViewModel
    {
        public DialogueTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<DialogueTabViewModel> logger)
            : base(viewService, dataSource, logger) { }

        protected override RPGDataEditor.Models.Dialogue CreateResource(PresentableData model) => new Models.Dialogue() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };

    }
}
