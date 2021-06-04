using MediatR;
using Microsoft.Extensions.Logging;
using Prism.Services.Dialogs;
using ResourceManager.Data;
using RPGDataEditor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPGDataEditor.Minecraft.Wpf.Npc.ViewModels
{
    public class NpcEditorViewModel : RPGDataEditor.Wpf.Npc.ViewModels.NpcEditorViewModel
    {
        public NpcEditorViewModel(IMediator mediator, ILogger<NpcEditorViewModel> logger) : base(mediator, logger) { }

        public override string Title => "Npc Editor";

        private double maxHealth;
        public double MaxHealth {
            get => maxHealth;
            set => SetProperty(ref maxHealth, value);
        }

        private double knockbackResistance;
        public double KnockbackResistance {
            get => knockbackResistance;
            set => SetProperty(ref knockbackResistance, value);
        }

        private double movementSpeed;
        public double MovementSpeed {
            get => movementSpeed;
            set => SetProperty(ref movementSpeed, value);
        }

        private double attackDamage;
        public double AttackDamage {
            get => attackDamage;
            set => SetProperty(ref attackDamage, value);
        }

        private double attackKnockback;
        public double AttackKnockback {
            get => attackKnockback;
            set => SetProperty(ref attackKnockback, value);
        }

        private double attackSpeed;
        public double AttackSpeed {
            get => attackSpeed;
            set => SetProperty(ref attackSpeed, value);
        }

        private double armor;
        public double Armor {
            get => armor;
            set => SetProperty(ref armor, value);
        }

        private double armorToughness;
        public double ArmorToughness {
            get => armorToughness;
            set => SetProperty(ref armorToughness, value);
        }

        private double respawnTime;
        public double RespawnTime {
            get => respawnTime;
            set => SetProperty(ref respawnTime, value);
        }

        protected override Task InitializeAsync(IDialogParameters parameters)
        {
            base.InitializeAsync(parameters);
            MaxHealth = GetFromAttributes("minecraft:generic.max_health");
            KnockbackResistance = GetFromAttributes("minecraft:generic.knockback_resistance");
            MovementSpeed = GetFromAttributes("minecraft:generic.movement_speed");
            AttackDamage = GetFromAttributes("minecraft:generic.attack_damage");
            AttackKnockback = GetFromAttributes("minecraft:generic.attack_knockback");
            AttackSpeed = GetFromAttributes("minecraft:generic.attack_speed");
            Armor = GetFromAttributes("minecraft:generic.armor");
            ArmorToughness = GetFromAttributes("minecraft:generic.armor_toughness");
            RespawnTime = GetFromAttributes("craftpolis:npc.respawn_time");
            return Task.CompletedTask;
        }

        public override Task OnDialogClosing(bool result)
        {
            if (result)
            {
                IList<AttributeData> attributes = Model.Attributes;
                attributes.Clear();
                attributes.Add(new AttributeData("minecraft:generic.max_health", MaxHealth));
                attributes.Add(new AttributeData("minecraft:generic.knockback_resistance", KnockbackResistance));
                attributes.Add(new AttributeData("minecraft:generic.movement_speed", MovementSpeed));
                attributes.Add(new AttributeData("minecraft:generic.attack_damage", AttackDamage));
                attributes.Add(new AttributeData("minecraft:generic.attack_knockback", AttackKnockback));
                attributes.Add(new AttributeData("minecraft:generic.attack_speed", AttackSpeed));
                attributes.Add(new AttributeData("minecraft:generic.armor", Armor));
                attributes.Add(new AttributeData("minecraft:generic.armor_toughness", ArmorToughness));
                attributes.Add(new AttributeData("craftpolis:npc.respawn_time", RespawnTime));
            }
            return Task.CompletedTask;
        }

    }
}
