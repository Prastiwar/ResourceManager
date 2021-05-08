using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : PresentableDataViewModel<Models.Npc>
    {
        public NpcTabViewModel(ViewModelContext context) : base(context) { }

        protected override PresentableData CreateModelInstance() => new PresentableNpc();
    }
}
