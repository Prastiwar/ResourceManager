using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Linq;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcDataModelEditorViewModel : ModelDialogViewModel<NpcDataModel>
    {
        public NpcDataModelEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Npc Editor";

        /// <summary> Finds attribute by name and returns its value </summary>
        protected double GetFromAttributes(string name)
        {
            AttributeDataModel attribute = Model.Attributes.Where(x => x.Name == name).FirstOrDefault();
            if (attribute != null)
            {
                return attribute.Value;
            }
            return 0;
        }

    }
}
