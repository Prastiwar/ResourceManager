using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Wpf;
using RPGDataEditor.Wpf.Views;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Minecraft.Wpf.Quest.Views
{
    public partial class QuestEditor : UserControl
    {
        public QuestEditor() => InitializeComponent();

        private void ToggleExpandIcon(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Commands.ToggleExpandIcon(btn);
        }

        private void RequirementView_TypeChange(object sender, RequirementView.ChangeTypeEventArgs e)
        {
            if (DataContext is ModelDialogViewModel<QuestModel> vm)
            {
                PlayerRequirementModel newModel = e.CreateRequirement();
                if (newModel != null)
                {
                    int index = vm.Model.Requirements.IndexOf(e.Requirement);
                    if (index > -1)
                    {
                        vm.Model.Requirements.RemoveAt(index);
                        vm.Model.Requirements.Insert(index, newModel);
                    }
                    ICollectionView view = CollectionViewSource.GetDefaultView(RequirementsListView.ItemsSource);
                    if (view != null)
                    {
                        view.Refresh();
                    }
                }
            }
        }
    }
}
