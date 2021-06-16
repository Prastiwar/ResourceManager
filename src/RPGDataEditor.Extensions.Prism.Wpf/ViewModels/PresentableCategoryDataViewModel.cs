using Microsoft.Extensions.Logging;
using Prism.Commands;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Mvvm.Services;
using System.Windows.Input;

namespace RPGDataEditor.Extensions.Prism.Wpf.ViewModels
{
    public abstract class PresentableCategoryDataViewModel<TResource> : Mvvm.PresentableCategoryDataViewModel<TResource> where TResource : ICategorizable
    {
        public PresentableCategoryDataViewModel(IViewService viewService, IDataSource dataSource, ILogger<Mvvm.PresentableCategoryDataViewModel<TResource>> logger)
            : base(viewService, dataSource, logger) { }

        public ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<PresentableData>(async presentable => await OpenEditorAsync(presentable));

        public ICommand removeModelCommand;
        public ICommand RemoveModelCommand => removeModelCommand ??= new DelegateCommand<PresentableData>(async presentable => await RemoveModelAsync(presentable));

        public ICommand addCategoryCommand;
        public ICommand AddCategoryCommand => addCategoryCommand ??= new DelegateCommand(() => CreateCategory());

        public ICommand showCategoryCommand;
        public ICommand ShowCategoryCommand => showCategoryCommand ??= new DelegateCommand<string>(category => ShowCategory(category));

        public ICommand removeCategoryCommand;
        public ICommand RemoveCategoryCommand => removeCategoryCommand ??= new DelegateCommand<string>(async category => await RemoveCategoryAsync(category));
    }
}
