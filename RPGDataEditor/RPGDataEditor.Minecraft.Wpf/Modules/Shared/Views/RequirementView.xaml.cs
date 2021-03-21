using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Validation;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Views
{
    public partial class RequirementView : UserControl
    {
        public delegate void ChangeTypeEventHandler(object sender, ChangeTypeEventArgs e);

        public class ChangeTypeEventArgs : RoutedEventArgs
        {
            public ChangeTypeEventArgs(PlayerRequirementModel requirement, string targetType)
            {
                Requirement = requirement;
                TargetType = targetType;
            }

            public PlayerRequirementModel Requirement { get; }
            public string TargetType { get; }
        }

        public RequirementView()
        {
            InitializeComponent();
            DataContextChanged += RequirementView_DataContextChanged;
        }

        public static DependencyProperty ChangeTypeRequestProperty =
            DependencyProperty.Register(nameof(ChangeTypeCommand), typeof(ICommand), typeof(RequirementView));
        public ICommand ChangeTypeCommand {
            get => (ICommand)GetValue(ChangeTypeRequestProperty);
            set => SetValue(ChangeTypeRequestProperty, value);
        }

        public static DependencyProperty ChangeTypeCommandParameterProperty =
            DependencyProperty.Register(nameof(ChangeTypeCommandParameter), typeof(object), typeof(RequirementView));
        public object ChangeTypeCommandParameter {
            get => GetValue(ChangeTypeCommandParameterProperty);
            set => SetValue(ChangeTypeCommandParameterProperty, value);
        }

        public static DependencyProperty ValidableContextProperty =
            DependencyProperty.Register(nameof(ValidableContext), typeof(IValidable), typeof(RequirementView));
        public IValidable ValidableContext {
            get => (IValidable)GetValue(ValidableContextProperty);
            set => SetValue(ValidableContextProperty, value);
        }

        public static readonly RoutedEvent TypeChangeEvent
            = EventManager.RegisterRoutedEvent("TypeChange", RoutingStrategy.Direct, typeof(ChangeTypeEventHandler), typeof(RequirementView));

        public event ChangeTypeEventHandler TypeChange {
            add => AddHandler(SizeChangedEvent, value, false);
            remove => RemoveHandler(SizeChangedEvent, value);
        }

        private void RequirementView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) =>
            SetRequirementPanel(DataContext as PlayerRequirementModel);

        private void QuestStage_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    if (DataContext is QuestRequirement requirement)
                    {
                        requirement.Stage = (QuestStage)Enum.Parse(typeof(QuestStage), selected.Name.ToString());
                    }
                }
            }
        }

        private void SetRequirementPanel(PlayerRequirementModel model)
        {
            bool isDialogue = model is DialogueRequirement;
            bool isQuest = model is QuestRequirement;
            bool isItem = model is ItemRequirement;
            RequirementType.SelectionChanged -= RequirementType_SelectionChanged;
            RequirementType.SelectedIndex = isDialogue ? 0 :
                                            isQuest ? 1 :
                                            2;
            RequirementType.SelectionChanged += RequirementType_SelectionChanged;
            DialogueRequirementPanel.Visibility = isDialogue ? Visibility.Visible : Visibility.Collapsed;
            QuestRequirementPanel.Visibility = isQuest ? Visibility.Visible : Visibility.Collapsed;
            ItemRequirementPanel.Visibility = isItem ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RequirementType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    ChangeTypeCommand?.Execute(ChangeTypeCommandParameter);
                    ChangeTypeEventArgs changeTypeArgs = new ChangeTypeEventArgs(DataContext as PlayerRequirementModel, selected.Name) {
                        RoutedEvent = TypeChangeEvent
                    };
                    RaiseEvent(changeTypeArgs);
                }
            }
        }
    }
}
