using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;

namespace RPGDataEditor.Minecraft.Wpf.Npc.ViewModels
{
    public class NpcTabViewModel : RPGDataEditor.Wpf.Npc.ViewModels.NpcTabViewModel
    {
        public NpcTabViewModel(ViewModelContext context,
                               ITypeToResourceConverter resourceConverter,
                               ILocationToSimpleResourceConverter simpleResourceConverter)
            : base(context, resourceConverter, simpleResourceConverter) { }

        protected override NpcDataModel CreateNewExactModel(SimpleIdentifiableData model) => new Models.NpcDataModel() {
            Id = model.Id,
            Name = model.Name
        };
    }
}
