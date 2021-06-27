using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Controls
{
    public delegate bool SearchPredicateEventHandler(object item, string searchText);

    public class SearchBox : TextBox
    {
        public static DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(SearchPredicateEventHandler), typeof(SearchBox));
        public SearchPredicateEventHandler Filter {
            get => (SearchPredicateEventHandler)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public static DependencyProperty ItemsSourceProperty 
            = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(SearchBox), new PropertyMetadata(null, OnItemsItemsSourceChanged));
        private static void OnItemsItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SearchBox).UpdateCollectionView();
        public IEnumerable ItemsSource {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        protected virtual void UpdateCollectionView()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);
            if (view != null)
            {
                view.Filter = (obj) => Filter == null || Filter.Invoke(obj, Text);
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            CollectionViewSource.GetDefaultView(ItemsSource)?.Refresh();
        }
    }
}
