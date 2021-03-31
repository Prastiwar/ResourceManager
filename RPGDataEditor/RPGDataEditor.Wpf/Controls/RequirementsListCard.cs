using MaterialDesignThemes.Wpf;
using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Providers;
using RPGDataEditor.Wpf.Converters;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Controls
{
    public class RequirementsListCard : UserControl
    {
        public static DependencyProperty RequirementsProperty =
            DependencyProperty.Register(nameof(Requirements), typeof(IList<PlayerRequirementModel>), typeof(RequirementsListCard),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public IList<PlayerRequirementModel> Requirements {
            get => (IList<PlayerRequirementModel>)GetValue(RequirementsProperty);
            set => SetValue(RequirementsProperty, value);
        }

        public static DependencyProperty AddRequirementCommandProperty =
            DependencyProperty.Register(nameof(AddRequirementCommand), typeof(ICommand), typeof(RequirementsListCard));
        public ICommand AddRequirementCommand {
            get => (ICommand)GetValue(AddRequirementCommandProperty);
            set => SetValue(AddRequirementCommandProperty, value);
        }

        public static DependencyProperty RemoveRequirementCommandProperty =
            DependencyProperty.Register(nameof(RemoveRequirementCommand), typeof(ICommand), typeof(RequirementsListCard));
        public ICommand RemoveRequirementCommand {
            get => (ICommand)GetValue(RemoveRequirementCommandProperty);
            set => SetValue(RemoveRequirementCommandProperty, value);
        }

        public static DependencyProperty ChangeRequirementTypeCommandProperty =
            DependencyProperty.Register(nameof(ChangeRequirementTypeCommand), typeof(ICommand), typeof(RequirementsListCard));
        public ICommand ChangeRequirementTypeCommand {
            get => (ICommand)GetValue(ChangeRequirementTypeCommandProperty);
            set => SetValue(ChangeRequirementTypeCommandProperty, value);
        }

        public static DependencyProperty ChangeRequirementTypeCommandParameterProperty =
            DependencyProperty.Register(nameof(ChangeRequirementTypeCommandParameter), typeof(object), typeof(RequirementsListCard));
        public object ChangeRequirementTypeCommandParameter {
            get => GetValue(ChangeRequirementTypeCommandParameterProperty);
            set => SetValue(ChangeRequirementTypeCommandParameterProperty, value);
        }

        public static DependencyProperty RequirementItemTemplateProperty =
            DependencyProperty.Register(nameof(RequirementItemTemplate), typeof(DataTemplate), typeof(RequirementsListCard));
        public DataTemplate RequirementItemTemplate {
            get => (DataTemplate)GetValue(RequirementItemTemplateProperty);
            set => SetValue(RequirementItemTemplateProperty, value);
        }

        public static DependencyProperty ValidablePathValuesBindingsProperty =
            DependencyProperty.Register(nameof(ValidablePathValuesBindings), typeof(MultiBindingValue), typeof(RequirementsListCard));
        public MultiBindingValue ValidablePathValuesBindings {
            get => (MultiBindingValue)GetValue(ValidablePathValuesBindingsProperty);
            set => SetValue(ValidablePathValuesBindingsProperty, value);
        }

        private ListView requirementsListView;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            requirementsListView = Template.FindName("RequirementsListView", this) as ListView;
            OnTemplateApplied();
        }

        protected virtual void OnTemplateApplied()
        {
            if (RequirementItemTemplate == null && GetBindingExpression(RequirementItemTemplateProperty) == null)
            {
                RequirementItemTemplate = TemplateGenerator.CreateDataTemplate(() => CreateRequirementItemTemplate());
            }
            if (RemoveRequirementCommand == null && GetBindingExpression(RemoveRequirementCommandProperty) == null)
            {
                RemoveRequirementCommand = Commands.RemoveListItemLiCommand(() => Requirements);
            }
            if (AddRequirementCommand == null && GetBindingExpression(AddRequirementCommandProperty) == null)
            {
                IModelProvider<PlayerRequirementModel> modelProvider = Application.Current.TryResolve<IModelProvider<PlayerRequirementModel>>();
                AddRequirementCommand = Commands.AddListItemCommand(() => Requirements, () => modelProvider.CreateModel("Dialogue"));
            }
        }

        protected virtual FrameworkElement CreateRequirementItemTemplate()
        {
            Card container = new Card() {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(5)
            };
            Grid grid = new Grid() {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            Button removeRequirmentButton = new Button() {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 12, 0, 0),
                Style = FindResource("DeleteFlatButtonStyle") as Style
            };
            removeRequirmentButton.SetBinding(Button.CommandProperty, new Binding(nameof(RemoveRequirementCommand)) { Source = this });
            removeRequirmentButton.SetBinding(Button.CommandParameterProperty, new Binding("."));
            grid.Children.Add(removeRequirmentButton);

            RequirementView requirementView = CreateRequirementView();
            grid.Children.Add(requirementView);
            Grid.SetColumn(requirementView, 1);
            container.Content = grid;
            return container;
        }

        protected virtual RequirementView CreateRequirementView()
        {
            RequirementView requirementView = new RequirementView() {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };
            requirementView.SetBinding(ChangeableUserControl.ChangeTypeRequestProperty, new Binding(nameof(ChangeRequirementTypeCommand)) { Source = this });
            requirementView.SetBinding(ChangeableUserControl.ChangeTypeCommandParameterProperty, new Binding(nameof(ChangeRequirementTypeCommandParameter)) { Source = this });
            requirementView.SetBinding(AttachProperties.ValidablePathFormatProperty, new Binding() {
                Path = new PropertyPath(AttachProperties.ValidablePathFormatProperty),
                Source = this
            });
            requirementView.SetBinding(AttachProperties.ValidableObjectProperty, new Binding() {
                Path = new PropertyPath(AttachProperties.ValidableObjectProperty),
                Source = this
            });

            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ValidablePathValuesBindingsProperty, GetType());
            dpd.AddValueChanged(this, (sender, e) => {
                BindingOperations.ClearBinding(requirementView, AttachProperties.ValidablePathValuesProperty);
                requirementView.SetBinding(AttachProperties.ValidablePathValuesProperty, ValidablePathValuesBindings.ToMultiBinding(new BindingListConverter()));
            });
            requirementView.SetBinding(AttachProperties.ValidablePathValuesProperty, ValidablePathValuesBindings.ToMultiBinding(new BindingListConverter()));
            requirementView.TypeChange += RequirementView_TypeChange;
            return requirementView;
        }

        protected virtual void RequirementView_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e) => e.ChangeTypeInList(Requirements, requirementsListView);

    }
}
