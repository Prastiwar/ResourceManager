using RpgDataEditor.Models;
using System;
using System.Windows.Controls;

namespace RpgDataEditor.Wpf.Views
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
            Npc npc = (Npc)item;
            if (int.TryParse(searchText, out int id))
            {
                return (int)npc.Id == id;
            }
            return npc.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }
    }
}
