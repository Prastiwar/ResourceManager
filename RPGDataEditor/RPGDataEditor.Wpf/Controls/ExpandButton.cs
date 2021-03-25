using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Controls
{
    public class ExpandButton : Button
    {
        public static DependencyProperty ExpandableElementProperty =
            DependencyProperty.Register(nameof(ExpandableElement), typeof(FrameworkElement), typeof(ExpandButton), new PropertyMetadata(null, OnExpandableElementPropertyChanged));
        private static void OnExpandableElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as ExpandButton).OnExpandableElementChanged((FrameworkElement)e.OldValue, (FrameworkElement)e.NewValue);
        public FrameworkElement ExpandableElement {
            get => (FrameworkElement)GetValue(ExpandableElementProperty);
            set => SetValue(ExpandableElementProperty, value);
        }

        public static DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(ExpandButton), new PropertyMetadata(false, OnIsExpandedPropertyChanged));
        private static void OnIsExpandedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as ExpandButton).OnIsExpandedChanged((bool)e.NewValue);
        public bool IsExpanded {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public static DependencyProperty ExpandMoreIconProperty =
            DependencyProperty.Register(nameof(ExpandMoreIcon), typeof(PackIconKind), typeof(ExpandButton), new PropertyMetadata(PackIconKind.ExpandMore));
        public PackIconKind ExpandMoreIcon {
            get => (PackIconKind)GetValue(ExpandMoreIconProperty);
            set => SetValue(ExpandMoreIconProperty, value);
        }

        public static DependencyProperty ExpandLessIconProperty =
            DependencyProperty.Register(nameof(ExpandLessIcon), typeof(PackIconKind), typeof(ExpandButton), new PropertyMetadata(PackIconKind.ExpandLess));
        public PackIconKind ExpandLessIcon {
            get => (PackIconKind)GetValue(ExpandLessIconProperty);
            set => SetValue(ExpandLessIconProperty, value);
        }

        protected virtual void OnExpandableElementChanged(FrameworkElement oldValue, FrameworkElement newValue)
        {
            if (oldValue is ItemsControl oldItemsControl)
            {
                (oldItemsControl.Items as INotifyPropertyChanged).PropertyChanged -= ExpandButton_PropertyChanged;
            }
            if (newValue is ItemsControl itemsControl)
            {
                bool hasBinding = GetBindingExpression(IsEnabledProperty) != null;
                if (!hasBinding)
                {
                    (itemsControl.Items as INotifyPropertyChanged).PropertyChanged -= ExpandButton_PropertyChanged;
                    (itemsControl.Items as INotifyPropertyChanged).PropertyChanged += ExpandButton_PropertyChanged;
                    IsEnabled = !itemsControl.Items.IsEmpty;
                }
            }
            OnIsExpandedChanged(IsExpanded);
        }

        private void ExpandButton_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool hasBinding = GetBindingExpression(IsEnabledProperty) != null;
            if (!hasBinding)
            {
                if (e.PropertyName == nameof(ItemsControl.Items.IsEmpty))
                {
                    IsEnabled = !(ExpandableElement as ItemsControl).Items.IsEmpty;
                }
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == ContentProperty)
            {
                SetContentIcon();
            }
        }

        protected virtual void OnIsExpandedChanged(bool isExpanded)
        {
            if (ExpandableElement != null)
            {
                ExpandableElement.Visibility = isExpanded ? Visibility.Visible : Visibility.Collapsed;
            }
            SetContentIcon();
        }

        private void SetContentIcon()
        {
            if (Content == null && GetBindingExpression(ContentProperty) == null)
            {
                Content = new PackIcon();
            }
            if (Content is PackIcon icon)
            {
                icon.Kind = IsExpanded ? ExpandLessIcon : ExpandMoreIcon;
            }
        }

        protected override void OnClick()
        {
            base.OnClick();
            IsExpanded = !IsExpanded;
        }
    }
}
