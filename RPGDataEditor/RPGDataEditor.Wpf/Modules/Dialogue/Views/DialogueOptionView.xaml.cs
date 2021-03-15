using RPGDataEditor.Core.Models;
using System.Windows.Controls;

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

        private void RequirementView_TypeChange(object sender, Wpf.Views.RequirementView.ChangeTypeEventArgs e)
        {
            if (DataContext is DialogueOptionModel model)
            {
                bool isDialogue = string.Compare(e.TargetType, "dialogue", true) == 0;
                bool isQuest = string.Compare(e.TargetType, "quest", true) == 0;
                bool isItem = string.Compare(e.TargetType, "item", true) == 0;
                PlayerRequirementModel newModel = null;
                if (isDialogue)
                {
                    newModel = new DialogueRequirement();
                }
                else if (isQuest)
                {
                    newModel = new QuestRequirement();
                }
                else if (isItem)
                {
                    newModel = new ItemRequirement();
                }
                if (newModel != null)
                {
                    int index = model.Requirements.IndexOf(e.Requirement);
                    if (index > -1)
                    {
                        model.Requirements.RemoveAt(index);
                        model.Requirements.Insert(index, newModel);
                    }
                }
            }
        }
    }
}
