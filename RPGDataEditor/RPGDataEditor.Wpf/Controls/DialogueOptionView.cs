using RPGDataEditor.Core.Models;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public class DialogueOptionView : UserControl
    {
        private ListView requirementsListView;
        private Button removeButton;
        private Button addRequirementButton;
        private RequirementView requirementView;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //DataContextChanged += DialogueOptionView_DataContextChanged;
            requirementsListView = Template.FindName("RequirementsListView", this) as ListView;
            removeButton = Template.FindName("RemoveButton", this) as Button;
            addRequirementButton = Template.FindName("AddRequirementButton", this) as Button;
            requirementView = Template.FindName("RequirementView", this) as RequirementView;

            removeButton.Click += OnRemoveRequirementClicked;
            addRequirementButton.Click += AddRequirement;
            requirementView.TypeChange += OnRequirementViewTypeChanged;
            //TypeComboBox.SelectionChanged += OptionType_Selected;
        }

        //private void DialogueOptionView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.NewValue is DialogueOptionModel model)
        //    {
        //        TypeComboBox.SelectedIndex = model.NextDialogId == -2 ? 1 :
        //                                     model.NextDialogId == -1 ? 2 :
        //                                     0;
        //    }
        //}

        //private void OptionType_Selected(object sender, SelectionChangedEventArgs e)
        //{
        //    if (e.AddedItems.Count > 0)
        //    {
        //        if (e.AddedItems[0] is ComboBoxItem selected)
        //        {
        //            bool isDialogue = string.Compare(selected.Name, "dialogue", true) == 0;
        //            bool isJob = string.Compare(selected.Name, "TriggerJob", true) == 0;
        //            bool isQuit = string.Compare(selected.Name, "quit", true) == 0;
        //            DialogueOptionPanel.Visibility = isDialogue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        //            if (DataContext is DialogueOptionModel model)
        //            {
        //                model.NextDialogId = isQuit ? -1 :
        //                                     isJob ? -2 :
        //                                     -1;
        //            }
        //        }
        //    }
        //}

        private void AddRequirement(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is DialogueOptionModel model)
            {
                model.Requirements.Add(new DialogueRequirement());
            }
        }

        private void OnRemoveRequirementClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is DialogueOptionModel model)
            {
                Button btn = (Button)sender;
                PlayerRequirementModel requirement = (PlayerRequirementModel)btn.DataContext;
                model.Requirements.Remove(requirement);
            }
        }

        private void OnRequirementViewTypeChanged(object sender, RequirementView.ChangeTypeEventArgs e)
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
                    ICollectionView view = CollectionViewSource.GetDefaultView(requirementsListView.ItemsSource);
                    if (view != null)
                    {
                        view.Refresh();
                    }
                }
            }
        }
    }
}
