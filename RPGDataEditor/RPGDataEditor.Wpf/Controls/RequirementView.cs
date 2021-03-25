using RPGDataEditor.Core.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Controls
{
    public class RequirementView : UserControl
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

        public static readonly RoutedEvent TypeChangeEvent
            = EventManager.RegisterRoutedEvent("TypeChange", RoutingStrategy.Direct, typeof(ChangeTypeEventHandler), typeof(RequirementView));

        public event ChangeTypeEventHandler TypeChange {
            add => AddHandler(SizeChangedEvent, value, false);
            remove => RemoveHandler(SizeChangedEvent, value);
        }

        private ComboBox requirementTypeComboBox;
        private ContentPresenter requirementContent;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            requirementTypeComboBox = Template.FindName("RequirementTypeComboBox", this) as ComboBox;
            requirementContent = Template.FindName("RequirementContent", this) as ContentPresenter;

            requirementTypeComboBox.SelectionChanged += RequirementType_SelectionChanged;
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
                    ApplyRequirementContent(selected.Name);
                }
            }
        }

        protected virtual void ApplyRequirementContent(string name)
        {
            object content = GetRequirementContent(name);
            requirementContent.Content = content;
        }

        protected virtual object GetRequirementContent(string name) => Application.Current.TryFindResource(name + "RequirementContent");
    }
}
