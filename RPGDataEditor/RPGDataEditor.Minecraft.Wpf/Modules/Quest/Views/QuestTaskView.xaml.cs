using RPGDataEditor.Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Minecraft.Wpf.Quest.Views
{
    public partial class QuestTaskView : UserControl
    {
        public QuestTaskView()
        {
            InitializeComponent();
            DataContextChanged += QuestTaskView_DataContextChanged;
        }

        private bool ignoreTaskTypeSelectionChanged;

        private void QuestTaskView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) =>
            SetTaskPanel(DataContext as QuestTask);

        private void SetTaskPanel(QuestTask model)
        {
            bool entityInteract = model is EntityInteractQuestTask;
            bool kill = model is KillQuestTask;
            bool reach = model is ReachQuestTask;
            bool blockInteract = model is BlockInteractQuestTask;
            bool itemInteract = model is RightItemInteractQuestTask;
            bool dialogue = model is DialogueQuestTask;

            ignoreTaskTypeSelectionChanged = true;
            TaskType.SelectedIndex = entityInteract ? 0 :
                                     kill ? 1 :
                                     reach ? 2 :
                                     blockInteract ? 4 :
                                     itemInteract ? 5 
                                     : 3;
            ignoreTaskTypeSelectionChanged = false;

            EntityInteractQuestTaskPanel.Visibility = entityInteract ? Visibility.Visible : Visibility.Collapsed;
            KillQuestTaskPanel.Visibility = kill ? Visibility.Visible : Visibility.Collapsed;
            ReachQuestTaskPanel.Visibility = reach ? Visibility.Visible : Visibility.Collapsed;
            BlockInteractQuestTaskPanel.Visibility = blockInteract ? Visibility.Visible : Visibility.Collapsed;
            ItemInteractQuestTaskPanel.Visibility = itemInteract ? Visibility.Visible : Visibility.Collapsed;
            DialogueQuestTaskPanel.Visibility = dialogue ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TaskType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ignoreTaskTypeSelectionChanged)
            {
                return;
            }
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    bool entityInteract = string.Compare(selected.Name, "EntityInteract", true) == 0;
                    bool kill = string.Compare(selected.Name, "Kill", true) == 0;
                    bool reach = string.Compare(selected.Name, "Reach", true) == 0;
                    bool blockInteract = string.Compare(selected.Name, "BlockInteract", true) == 0;
                    bool itemInteract = string.Compare(selected.Name, "ItemInteract", true) == 0;
                    bool dialogue = string.Compare(selected.Name, "Dialogue", true) == 0;

                    if (entityInteract)
                    {
                        DataContext = new EntityInteractQuestTask();
                    }
                    else if (kill)
                    {
                        DataContext = new KillQuestTask();
                    }
                    else if (reach)
                    {
                        DataContext = new ReachQuestTask();
                    }
                    else if (blockInteract)
                    {
                        DataContext = new RightBlockInteractQuestTask();
                    }
                    else if (itemInteract)
                    {
                        DataContext = new RightItemInteractQuestTask();
                    }
                    else if (dialogue)
                    {
                        DataContext = new DialogueQuestTask();
                    }
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Position previousPos = DataContext is BlockInteractQuestTask previousTask ? previousTask.Pos : new Position();

            DataContext = new LeftBlockInteractQuestTask() {
                Pos = previousPos
            };
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Position previousPos = DataContext is BlockInteractQuestTask previousTask ? previousTask.Pos : new Position();

            DataContext = new RightBlockInteractQuestTask() {
                Pos = previousPos
            };
        }
    }
}
