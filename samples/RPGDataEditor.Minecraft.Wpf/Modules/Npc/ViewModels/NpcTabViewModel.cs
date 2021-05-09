using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;

namespace RPGDataEditor.Minecraft.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : RPGDataEditor.Wpf.Npc.ViewModels.NpcTabViewModel
    {
        public NpcTabViewModel(ViewModelContext context) : base(context) { }

        protected override RPGDataEditor.Models.Npc CreateResource(PresentableData model) => new Models.Npc() {
            Id = model.Id,
            Name = model.Name
        };
    }
}
