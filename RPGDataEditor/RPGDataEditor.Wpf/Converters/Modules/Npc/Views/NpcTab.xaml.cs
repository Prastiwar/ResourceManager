using RPGDataEditor.Core.Models;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace RPGDataEditor.Wpf.Npc.Views
{
    public partial class NpcTab : UserControl
    {
        public NpcTab()
        {
            InitializeComponent();
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ListView));
            dpd.AddValueChanged(NpcsList, OnNpcListBindingChanged);
            NpcsList.Focus();
        }

        private void OnNpcListBindingChanged(object sender, EventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(NpcsList.ItemsSource);
            if (view != null)
            {
                view.Filter = NpcFilter;
            }
        }

        private bool NpcFilter(object item)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                return true;
            }
            string name = SearchTextBox.Text;
            SimpleIdentifiableData npc = (SimpleIdentifiableData)item;
            if (int.TryParse(name, out int id))
            {
                return npc.Id == id;
            }
            return npc.Name.Contains(name, StringComparison.OrdinalIgnoreCase);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => CollectionViewSource.GetDefaultView(NpcsList.ItemsSource).Refresh();
    }
}
