using MaterialDesignThemes.Wpf;
using RPGDataEditor.Core.Models;
using System.Collections.Generic;
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

        private ListView requirementsListView;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            requirementsListView = Template.FindName("RequirementsListView", this) as ListView;
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
                //AddRequirementCommand = Commands.AddListItemCommand(() => Requirements, );
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
                Style = Application.Current.Resources.FindName("DeleteFlatButtonStyle") as Style
            };
            removeRequirmentButton.SetBinding(Button.CommandProperty, new Binding(nameof(RemoveRequirementCommand)) { Source = this });
            removeRequirmentButton.SetBinding(Button.CommandParameterProperty, new Binding("."));
            grid.Children.Add(removeRequirmentButton);

            RequirementView requirementView = new RequirementView() {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };
            requirementView.SetBinding(DataContextProperty, new Binding(nameof(DataContext)) {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ListBoxItem), 1)
            });
            requirementView.SetBinding(RequirementView.ChangeTypeRequestProperty, new Binding(nameof(ChangeRequirementTypeCommand)) { Source = this });
            requirementView.SetBinding(RequirementView.ChangeTypeCommandParameterProperty, new Binding(nameof(ChangeRequirementTypeCommandParameter)) { Source = this });
            requirementView.SetBinding(AttachProperties.ValidablePathFormatProperty, new Binding() {
                Path = new PropertyPath(AttachProperties.ValidablePathFormatProperty),
                Source = this
            });
            requirementView.SetBinding(AttachProperties.ValidableObjectProperty, new Binding() {
                Path = new PropertyPath(AttachProperties.ValidableObjectProperty),
                Source = this
            });
            requirementView.TypeChange += RequirementView_TypeChange;
            //AttachProperties.SetValidablePathValues(requirementView);
            grid.Children.Add(requirementView);
            Grid.SetColumn(requirementView, 1);
            container.Content = grid;
            return container;
            /*
                 <controls:RequirementView>
                     <local:AttachProperties.ValidablePathValues>
                         <MultiBinding Converter="{StaticResource BindingListConverter}">
                             <Binding Path="(ItemsControl.AlternationIndex)"
                                      RelativeSource="{RelativeSource AncestorType={x:Type ListBoxItem}}" />
                         </MultiBinding>
                     </local:AttachProperties.ValidablePathValues>
                 </controls:RequirementView>
             */
        }

        protected virtual void RequirementView_TypeChange(object sender, ChangeableUserControl.ChangeTypeEventArgs e) => e.ChangeTypeInList(Requirements, requirementsListView);

    }
}
