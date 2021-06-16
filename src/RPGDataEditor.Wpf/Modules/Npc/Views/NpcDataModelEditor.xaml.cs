using RPGDataEditor.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Npc.Views
{
    public partial class NpcEditor : UserControl
    {
        public NpcEditor()
        {
            AddInitiationDialogue = Commands.AddListItemCommand(() => InitiationDialoguesListDataCard.ItemsSource, () => -1);
            InitializeComponent();
        }

        public ICommand AddInitiationDialogue { get; }

        private void JobComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ComboBoxItem selected)
                {
                    bool isTrader = string.Compare(selected.Name, "trader", true) == 0;
                    RefreshJobPanelVisibility(selected.Name);
                    ComboBox comboBox = (ComboBox)sender;
                    if (comboBox.DataContext is Models.Npc model)
                    {
                        if (isTrader)
                        {
                            model.Job = new TraderNpcJob() {
                                Items = new ObservableCollection<TradeItem>()
                            };
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
            bool isTrader = string.Compare(jobName, "trader", true) == 0;
            TraderStackPanel.Visibility = isTrader ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void JobComboBox_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            comboBox.SelectionChanged -= JobComboBox_SelectionChanged;
            if (comboBox.DataContext is Models.Npc model)
            {
                comboBox.SelectedIndex = model.Job == null
                                         ? 0 : 1;
                RefreshJobPanelVisibility((comboBox.SelectedItem as ComboBoxItem).Name);
            }
            comboBox.SelectionChanged += JobComboBox_SelectionChanged;
        }
    }
}
