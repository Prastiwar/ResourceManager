using System.Collections.Generic;
using System.Collections.ObjectModel;
using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Minecraft.Models
{
    public class McDialogueOptionModel : DialogueOptionModel
    {
        private string command = "";
        public string Command {
            get => command;
            set => SetProperty(ref command, value ?? "");
        }

        private int color = 1;
        public int Color {
            get => color;
            set => SetProperty(ref color, value);
        }

        private IList<PlayerRequirementModel> requirements = new ObservableCollection<PlayerRequirementModel>();
        public IList<PlayerRequirementModel> Requirements {
            get => requirements;
            set => SetProperty(ref requirements, value ?? new ObservableCollection<PlayerRequirementModel>());
        }
    }
}
