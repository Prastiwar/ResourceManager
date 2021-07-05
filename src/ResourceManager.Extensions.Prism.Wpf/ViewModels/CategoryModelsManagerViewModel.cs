using Microsoft.Extensions.Logging;
using Prism.Commands;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Mvvm.Services;
using System.Windows.Input;

namespace ResourceManager.Extensions.Prism.Wpf.ViewModels
{
    public abstract class CategoryModelsManagerViewModel<TResource> : Mvvm.CategoryModelsManagerViewModel<TResource> where TResource : ICategorizable
    {
        public CategoryModelsManagerViewModel(IViewService viewService, IDataSource dataSource, ILogger<CategoryModelsManagerViewModel<TResource>> logger)
            : base(viewService, dataSource, logger) { }

        private ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<TResource>(async presentable => await OpenEditorAsync(presentable));

        private ICommand removeModelCommand;
        public ICommand RemoveModelCommand => removeModelCommand ??= new DelegateCommand<TResource>(async presentable => await RemoveModelAsync(presentable));

        private ICommand addCategoryCommand;
        public ICommand AddCategoryCommand => addCategoryCommand ??= new DelegateCommand(() => CreateCategory());

        private ICommand showCategoryCommand;
        public ICommand ShowCategoryCommand => showCategoryCommand ??= new DelegateCommand<string>(category => ShowCategory(category));

        private ICommand removeCategoryCommand;
        public ICommand RemoveCategoryCommand => removeCategoryCommand ??= new DelegateCommand<string>(async category => await RemoveCategoryAsync(category));

        private ICommand addModelCommand;
        public ICommand AddModelCommand => addModelCommand ??= new DelegateCommand<string>(async category => await CreateModelAsync());
    }
}
