using RPGDataEditor.Models;
using System.Collections.Generic;

namespace RPGDataEditor.Minecraft.Models
{
    public class Npc : RPGDataEditor.Models.Npc
    {
        public string Title { get; set; }

        public bool Invulnerable { get; set; }

        public bool LookAtPlayer { get; set; }

        public string DeathSoundLocation { get; set; }

        public string HurtSoundLocation { get; set; }

        public string AmbientSoundLocation { get; set; }

        public string TextureLocation { get; set; }

        public NpcMovement MovementType { get; set; }

        public IList<Position> Paths { get; set; }

        public int HealthRegen { get; set; }

        public Equipment Equipment { get; set; }
    }
}
