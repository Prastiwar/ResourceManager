using Microsoft.Extensions.Logging;
using Prism.Commands;
using ResourceManager.Data;
using ResourceManager.DataSource;
using RPGDataEditor.Mvvm.Services;
using System.Windows.Input;

namespace RPGDataEditor.Extensions.Prism.Wpf.ViewModels
{
    public abstract class PresentableDataViewModel<TResource> : Mvvm.PresentableDataViewModel<TResource> where TResource : IIdentifiable
    {
        public PresentableDataViewModel(IViewService viewService, IDataSource dataSource, ILogger<PresentableDataViewModel<TResource>> logger)
            : base(viewService, dataSource, logger) { }

        public ICommand openEditorCommand;
        public ICommand OpenEditorCommand => openEditorCommand ??= new DelegateCommand<PresentableData>(async presentable => await OpenEditorAsync(presentable));

        public ICommand removeModelCommand;
        public ICommand RemoveModelCommand => removeModelCommand ??= new DelegateCommand<PresentableData>(async presentable => await RemoveModelAsync(presentable));
    }
}
