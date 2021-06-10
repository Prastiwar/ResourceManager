using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Services;
using RPGDataEditor.Mvvm.Services;

namespace RPGDataEditor.Minecraft.Wpf.Dialogue.ViewModels
{
    public class DialogueTabViewModel : RPGDataEditor.Wpf.Dialogue.ViewModels.DialogueTabViewModel
    {
        public DialogueTabViewModel(IResourceDescriptorService resourceService, IViewService viewService, IDataSource dataSource, ILogger<DialogueTabViewModel> logger)
            : base(resourceService, viewService, dataSource, logger) { }

        protected override RPGDataEditor.Models.Dialogue CreateResource(PresentableData model) => new Models.Dialogue() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };

    }
}
