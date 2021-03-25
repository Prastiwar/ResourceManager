using RPGDataEditor.Core.Models;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Dialogue.Views
{
    public partial class DialogueOptionView : UserControl
    {
        public DialogueOptionView()
        {
            InitializeComponent();
            DataContextChanged += DialogueOptionView_DataContextChanged;
            TypeComboBox.SelectionChanged += OptionType_Selected;
        }

        private void ToggleExpandIcon(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Commands.ToggleExpandIcon(btn);
        }

        private void DialogueOptionView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DialogueOptionModel model)
            {
                TypeComboBox.SelectedIndex = model.NextDialogId == -2 ? 1 :
                                             model.NextDialogId == -1 ? 2 :
                                             0;
            }
        }

        private void OptionType_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    bool isDialogue = string.Compare(selected.Name, "dialogue", true) == 0;
                    bool isJob = string.Compare(selected.Name, "TriggerJob", true) == 0;
                    bool isQuit = string.Compare(selected.Name, "quit", true) == 0;
                    DialogueOptionPanel.Visibility = isDialogue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                    if (DataContext is DialogueOptionModel model)
                    {
                        model.NextDialogId = isQuit ? -1 :
                                             isJob ? -2 :
                                             -1;
                    }
                }
            }
        }

        private void AddRequirement(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is DialogueOptionModel model)
            {
                model.Requirements.Add(new DialogueRequirement());
            }
        }

        private void RemoveRequirement(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is DialogueOptionModel model)
            {
                Button btn = (Button)sender;
                PlayerRequirementModel requirement = (PlayerRequirementModel)btn.DataContext;
                model.Requirements.Remove(requirement);
            }
        }

        private void RequirementView_TypeChange(object sender, Controls.RequirementView.ChangeTypeEventArgs e)
        {
            if (DataContext is DialogueOptionModel model)
            {
                PlayerRequirementModel newModel = e.CreateRequirement();
                if (newModel != null)
                {
                    int index = model.Requirements.IndexOf(e.Requirement);
                    if (index > -1)
                    {
                        model.Requirements.RemoveAt(index);
                        model.Requirements.Insert(index, newModel);
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
