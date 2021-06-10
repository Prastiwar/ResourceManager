using FluentValidation;
using Microsoft.Extensions.Logging;
using RPGDataEditor.Models;
using RPGDataEditor.Mvvm;
using System.Linq;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcEditorViewModel : ModelDialogViewModel<Models.Npc>
    {
        public NpcEditorViewModel(IValidator<Models.Npc> validator, ILogger<NpcEditorViewModel> logger) : base(validator, logger) { }

        public override string Title => "Npc Editor";

        /// <summary> Finds attribute by name and returns its value </summary>
        protected double GetFromAttributes(string name)
        {
            AttributeData attribute = Model.Attributes.FirstOrDefault(x => x.Name == name);
            if (attribute != null)
            {
                return attribute.Value;
            }
            return 0;
        }

    }
}
