using ResourceManager.Wpf;
using System.Windows.Controls;
using System.Windows.Input;

namespace RpgDataEditor.Wpf.Views
{
   public partial class NpcEditor : UserControl
   {
       public NpcEditor()
       {
           AddInitiationDialogue = Commands.AddListItemCommand(() => InitiationDialoguesListDataCard.ItemsSource, () => -1);
           InitializeComponent();
       }

       public ICommand AddInitiationDialogue { get; }
   }
}
