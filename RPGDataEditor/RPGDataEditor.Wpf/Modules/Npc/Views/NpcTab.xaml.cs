using RPGDataEditor.Core.Models;
using System;
using System.Windows.Controls;

namespace RPGDataEditor.Wpf.Npc.Views
{
    public partial class NpcTab : UserControl
    {
        public NpcTab()
        {
            InitializeComponent();
            NpcsList.Focus();
        }

        private bool NpcFilter(object item, string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return true;
            }
            SimpleIdentifiableData npc = (SimpleIdentifiableData)item;
            if (int.TryParse(searchText, out int id))
            {
                return npc.Id == id;
            }
            return npc.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }
    }
}
