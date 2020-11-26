using RPGDataEditor.Core.Models;
using System;
using System.Windows.Controls;
using System.Windows;

namespace RPGDataEditor.Wpf.Views
{
    public partial class RequirementView : UserControl
    {
        public RequirementView() => InitializeComponent();

        private void QuestStage_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    if (DataContext is PlayerRequirementBuilder builder && builder.Model is QuestRequirement requirement)
                    {
                        requirement.Stage = (QuestStage)Enum.Parse(typeof(QuestStage), selected.Name.ToString());
                    }
                }
            }
        }

        private void RequirementType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    bool isDialogue = string.Compare(selected.Name, "dialogue", true) == 0;
                    bool isQuest = string.Compare(selected.Name, "quest", true) == 0;
                    bool isItem = string.Compare(selected.Name, "item", true) == 0;
                    DialogueRequirementPanel.Visibility = isDialogue ? Visibility.Visible : Visibility.Collapsed;
                    QuestRequirementPanel.Visibility = isQuest ? Visibility.Visible : Visibility.Collapsed;
                    ItemRequirementPanel.Visibility = isItem ? Visibility.Visible : Visibility.Collapsed;
                    if (DataContext is PlayerRequirementBuilder model)
                    {
                        if (isDialogue)
                        {
                            model.Model = new DialogueRequirement();
                        }
                        else if (isQuest)
                        {
                            model.Model = new QuestRequirement();
                        }
                        else if (isItem)
                        {
                            model.Model = new ItemRequirement();
                        }
                    }
                }
            }
        }
    }
}
}
