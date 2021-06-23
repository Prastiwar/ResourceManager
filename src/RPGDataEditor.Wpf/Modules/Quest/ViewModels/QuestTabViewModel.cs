﻿using Microsoft.Extensions.Logging;
using ResourceManager;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Extensions.Prism.Wpf.ViewModels;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm.Services;
using System.Collections.Generic;
using System.Linq;

namespace RPGDataEditor.Wpf.Quest.ViewModels
{
    public class QuestTabViewModel : PresentableCategoryDataViewModel<Models.Quest>
    {
        public QuestTabViewModel(IViewService viewService, IDataSource dataSource, ILogger<QuestTabViewModel> logger)
            : base(viewService, dataSource, logger) { }

        protected override PresentableData CreateModelInstance() => new PresentableQuest() { Category = CurrentCategory };

        protected override Models.Quest CreateResource(PresentableData model) => new Models.Quest() {
            Id = model.Id,
            Title = model.Name,
            Category = (model as PresentableCategoryData).Category
        };

        protected override PresentableData CreatePresentableData(string location)
        {
            PresentableCategoryData presentable = (PresentableCategoryData)CreateModelInstance();
            LocationResourceDescriptor pathDescriptor = DataSource.DescriptorService.GetRequiredDescriptor<LocationResourceDescriptor>(typeof(Models.Quest));
            KeyValuePair<string, object>[] parameters = pathDescriptor.ParseParameters(location);
            presentable.Id = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableData.Id), true) == 0).Value;
            presentable.Name = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(RPGDataEditor.Models.Quest.Title), true) == 0).Value?.ToString();
            presentable.Category = parameters.FirstOrDefault(x => string.Compare(x.Key, nameof(PresentableCategoryData.Category), true) == 0).Value?.ToString();
            return presentable;
        }
    }
}
