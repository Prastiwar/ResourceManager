using RPGDataEditor.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGDataEditor.Minecraft.Models
{
    public class McNpcDataModel : NpcDataModel
    {
        private string title = "";
        public string Title {
            get => title;
            set => SetProperty(ref title, value ?? "");
        }

        private bool invulnerable;
        public bool Invulnerable {
            get => invulnerable;
            set => SetProperty(ref invulnerable, value);
        }

        private bool lookAtPlayer;
        public bool LookAtPlayer {
            get => lookAtPlayer;
            set => SetProperty(ref lookAtPlayer, value);
        }

        private string deathSoundLocation = "";
        public string DeathSoundLocation {
            get => deathSoundLocation;
            set => SetProperty(ref deathSoundLocation, value ?? "");
        }

        private string hurtSoundLocation = "";
        public string HurtSoundLocation {
            get => hurtSoundLocation;
            set => SetProperty(ref hurtSoundLocation, value ?? "");
        }

        private string ambientSoundLocation = "";
        public string AmbientSoundLocation {
            get => ambientSoundLocation;
            set => SetProperty(ref ambientSoundLocation, value ?? "");
        }

        private string textureLocation = "";
        public string TextureLocation {
            get => textureLocation;
            set => SetProperty(ref textureLocation, value ?? "");
        }

        private NpcMovement movementType;
        public NpcMovement MovementType {
            get => movementType;
            set => SetProperty(ref movementType, value);
        }

        private IList<Position> paths = new ObservableCollection<Position>();
        public IList<Position> Paths {
            get => paths;
            set => SetProperty(ref paths, value ?? new ObservableCollection<Position>());
        }

        private int healthRegen;
        public int HealthRegen {
            get => healthRegen;
            set => SetProperty(ref healthRegen, value);
        }

        private EquipmentModel equipment = new EquipmentModel();
        public EquipmentModel Equipment {
            get => equipment;
            set => SetProperty(ref equipment, value ?? new EquipmentModel());
        }
    }
}
