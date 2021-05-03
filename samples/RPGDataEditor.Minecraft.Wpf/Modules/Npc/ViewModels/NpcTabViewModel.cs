using RPGDataEditor.Core;
using RPGDataEditor.Models;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Minecraft.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : RPGDataEditor.Wpf.Npc.ViewRPGDataEditor.Models.NpcTabViewModel
    {
        public NpcTabViewModel(ViewModelContext context,
                               ITypeToResourceConverter resourceConverter,
                               ILocationToSimpleResourceConverter simpleResourceConverter)
            : base(context, resourceConverter, simpleResourceConverter) { }

        protected override Npc CreateNewExactModel(SimpleIdentifiableData model) => new RPGDataEditor.Models.Npc() {
            Id = model.Id,
            Name = model.Name
        };
    }
}
