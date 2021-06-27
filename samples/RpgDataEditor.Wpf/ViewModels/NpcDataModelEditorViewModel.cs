using FluentValidation;
using Microsoft.Extensions.Logging;
using ResourceManager.Mvvm;
using RpgDataEditor.Models;
using System.Linq;

namespace RpgDataEditor.Wpf.ViewModels
{
    public class NpcEditorViewModel : ModelDialogViewModel<Npc>
    {
        public NpcEditorViewModel(IValidator<Npc> validator, ILogger<NpcEditorViewModel> logger) : base(validator, logger) { }

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
