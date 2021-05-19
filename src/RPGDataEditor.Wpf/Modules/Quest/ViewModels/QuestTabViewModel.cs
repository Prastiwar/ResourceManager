using MediatR;
using Microsoft.Extensions.Logging;
using ResourceManager.Data;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestTabViewModel : PresentableCategoryDataViewModel<Models.Quest>
    {
        public QuestTabViewModel(IMediator mediator, ILogger<QuestTabViewModel> logger) : base(mediator, logger) { }

        protected override PresentableData CreateModelInstance() => new PresentableQuest() { Category = CurrentCategory };

        protected override Models.Quest CreateResource(PresentableData model) => new Models.Quest() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };
    }
}
