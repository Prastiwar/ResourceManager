using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Views
{
    public partial class ItemSelectionList : UserControl
    {
        public ItemSelectionList() => InitializeComponent();

        public static DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IEnumerable<object>), typeof(ItemSelectionList));

        public IEnumerable<object> Items {
            get => (IEnumerable<object>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);  
        }
    }
}
