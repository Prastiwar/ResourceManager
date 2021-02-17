using Prism.Services.Dialogs;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcEditorViewModel : ModelDialogViewModel<NpcDataModel>
    {
        public NpcEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Npc Editor";

        public ICommand AddPathCommand => Commands.AddListItemLiCommand(() => Model.Paths);
        public ICommand RemovePathCommand => Commands.RemoveListItemLiCommand(() => Model.Paths);
        public int PathsCount => Model == null ? 0 : Model.Paths.Count;

        protected override Task InitializeAsync(IDialogParameters parameters)
        {
            base.InitializeAsync(parameters);
            if (Model.Paths is INotifyCollectionChanged optionsNotifier)
            {
                optionsNotifier.CollectionChanged += Paths_CollectionChanged;
            }
            return Task.CompletedTask;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(Model))
            {
                RaisePropertyChanged(nameof(PathsCount));
            }
        }

        private void Paths_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => RaisePropertyChanged(nameof(PathsCount));

    }
}
