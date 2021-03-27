using RPGDataEditor.Core.Models;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Controls
{
    public class DialogueOptionView : UserControl
    {
        private ListView requirementsListView;
        private Button removeButton;
        private Button addRequirementButton;
        private RequirementView requirementView;
        private ComboBox typeComboBox;
        private ResourcePicker nextDialoguePicker;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DataContextChanged += DialogueOptionView_DataContextChanged;
            requirementsListView = Template.FindName("RequirementsListView", this) as ListView;
            //removeButton = Template.FindName("RemoveButton", this) as Button;
            addRequirementButton = Template.FindName("AddRequirementButton", this) as Button;
            //requirementView = Template.FindName("RequirementView", this) as RequirementView;
            typeComboBox = Template.FindName("TypeComboBox", this) as ComboBox;
            nextDialoguePicker = Template.FindName("NextDialoguePicker", this) as ResourcePicker;

            //removeButton.Click += OnRemoveRequirementClicked;
            addRequirementButton.Click += AddRequirement;
            //requirementView.TypeChange += OnRequirementViewTypeChanged;
            typeComboBox.SelectionChanged += OptionType_Selected;
        }

        private void DialogueOptionView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DialogueOptionModel model)
            {
                typeComboBox.SelectedIndex = model.NextDialogId == -2 ? 1 :
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
                    nextDialoguePicker.Visibility = isDialogue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
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

        private void OnRemoveRequirementClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is DialogueOptionModel model)
            {
                Button btn = (Button)sender;
                PlayerRequirementModel requirement = (PlayerRequirementModel)btn.DataContext;
                model.Requirements.Remove(requirement);
            }
        }

        private void OnRequirementViewTypeChanged(object sender, ChangeableUserControl.ChangeTypeEventArgs e)
        {
            if (DataContext is DialogueOptionModel model)
            {
                e.ChangeTypeInList(model.Requirements, requirementsListView);
            }
        }
    }
}
