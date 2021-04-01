using Prism.Services.Dialogs;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGDataEditor.Wpf.Npc.ViewModels
{
    public class NpcDataModelEditorViewModel : ModelDialogViewModel<NpcDataModel>
    {
        public NpcDataModelEditorViewModel(ViewModelContext context) : base(context) { }

        public override string Title => "Npc Editor";

        public ICommand AddPathCommand => Commands.AddListItemCommand(() => Model.Paths);
        public ICommand RemovePathCommand => Commands.RemoveListItemCommand(() => Model.Paths);
        public int PathsCount => Model == null ? 0 : Model.Paths.Count;

        public ICommand AddTalkLineCommand => Commands.AddParameterListItemCommand<TalkLine>();
        public ICommand RemoveTalkLineCommand => Commands.RemoveItemFromListView();

        public ICommand AddDialogueIdCommand => Commands.AddParameterListItemCommand<int>();
        
        public ICommand AddTradeItemCommand => Commands.AddListItemCommand(() => ((TraderNpcJobModel)Model.Job).Items);
        public ICommand RemoveTradeItemCommand => Commands.RemoveItemFromListView();

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
            if (Model.Paths is INotifyCollectionChanged optionsNotifier)
            {
                optionsNotifier.CollectionChanged += Paths_CollectionChanged;
            }
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

        private double GetFromAttributes(string name)
        {
            AttributeDataModel attribute = Model.Attributes.Where(x => x.Name == name).FirstOrDefault();
            if (attribute != null)
            {
                return attribute.Value;
            }
            return 0;
        }

        public override Task OnDialogClosing(bool result)
        {
            if (result)
            {
                Model.Attributes.Clear();
                Model.Attributes.Add(new AttributeDataModel("minecraft:generic.max_health", MaxHealth));
                Model.Attributes.Add(new AttributeDataModel("minecraft:generic.knockback_resistance", KnockbackResistance));
                Model.Attributes.Add(new AttributeDataModel("minecraft:generic.movement_speed", MovementSpeed));
                Model.Attributes.Add(new AttributeDataModel("minecraft:generic.attack_damage", AttackDamage));
                Model.Attributes.Add(new AttributeDataModel("minecraft:generic.attack_knockback", AttackKnockback));
                Model.Attributes.Add(new AttributeDataModel("minecraft:generic.attack_speed", AttackSpeed));
                Model.Attributes.Add(new AttributeDataModel("minecraft:generic.armor", Armor));
                Model.Attributes.Add(new AttributeDataModel("minecraft:generic.armor_toughness", ArmorToughness));
                Model.Attributes.Add(new AttributeDataModel("craftpolis:npc.respawn_time", RespawnTime));
            }
            return Task.CompletedTask;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(Model))
            {
                RaisePropertyChanged(nameof(PathsCount));
            }
        }

        private void Paths_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => RaisePropertyChanged(nameof(PathsCount));

    }
}
