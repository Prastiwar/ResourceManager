using Microsoft.Extensions.Logging;
using Prism.Commands;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Mvvm.Services;
using System.Windows.Input;

namespace RPGDataEditor.Extensions.Prism.Wpf.ViewModels
{
    public abstract class ModelsManagerViewModel<TResource> : Mvvm.ModelsManagerViewModel<TResource> where TResource : IIdentifiable
    {
        public ModelsManagerViewModel(IViewService viewService, IDataSource dataSource, ILogger<ModelsManagerViewModel<TResource>> logger)
            : base(viewService, dataSource, logger) { }

        public ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<TResource>(async presentable => await OpenEditorAsync(presentable));

        public ICommand removeModelCommand;
        public ICommand RemoveModelCommand => removeModelCommand ??= new DelegateCommand<TResource>(async presentable => await RemoveModelAsync(presentable));
    }
}
