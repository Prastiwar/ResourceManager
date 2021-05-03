﻿using RPGDataEditor.Models;
using RPGDataEditor.Minecraft.Models;
using System.Windows.Controls;

namespace RPGDataEditor.Minecraft.Wpf.Npc.Views
{
    public partial class NpcEditor : UserControl
    {
        public NpcEditor() => InitializeComponent();

        private void JobComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    bool isGuard = string.Compare(selected.Name, "guard", true) == 0;
                    bool isTrader = string.Compare(selected.Name, "trader", true) == 0;
                    RefreshJobPanelVisibility(selected.Name);
                    ComboBox comboBox = (ComboBox)sender;
                    if (comboBox.DataContext is RPGDataEditor.Models.Npc model)
                    {
                        if (isGuard)
                        {
                            model.Job = new GuardNpcJob();
                        }
                        else if (isTrader)
                        {
                            model.Job = new TraderNpcJob();
                        }
                        else
                        {
                            model.Job = null;
                        }
                    }
                }
            }
        }

        private void RefreshJobPanelVisibility(string jobName)
        {
            bool isGuard = string.Compare(jobName, "guard", true) == 0;
            bool isTrader = string.Compare(jobName, "trader", true) == 0;
            TraderStackPanel.Visibility = isTrader ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            GuardStackPanel.Visibility = isGuard ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void JobComboBox_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            comboBox.SelectionChanged -= JobComboBox_SelectionChanged;
            if (comboBox.DataContext is RPGDataEditor.Models.Npc model)
            {
                comboBox.SelectedIndex = model.Job == null
                                         ? 0 : model.Job is GuardNpcJob
                                         ? 1 : 2;
                RefreshJobPanelVisibility((comboBox.SelectedItem as ComboBoxItem).Name);
            }
            comboBox.SelectionChanged += JobComboBox_SelectionChanged;
        }
    }
}
