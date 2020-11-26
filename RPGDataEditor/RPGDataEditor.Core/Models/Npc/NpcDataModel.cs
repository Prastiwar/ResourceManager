﻿namespace RPGDataEditor.Core.Models
{
    public class NpcDataModel : ObservableModel, IIdentifiable
    {
        private int id;
        public int GetId() => id;
        public void SetId(int value) => id = value;

        private string name = "";
        public string Name {
            get => name;
            set => SetProperty(ref name, value ?? "");
        }

        private Position pos = new Position();
        public Position Pos {
            get => pos;
            set => SetProperty(ref pos, value ?? new Position());
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

        private TalkDataModel talkData = new TalkDataModel();
        public TalkDataModel TalkData {
            get => talkData;
            set => SetProperty(ref talkData, value ?? new TalkDataModel());
        }

        private NpcJobModel job;
        public NpcJobModel Job {
            get => job;
            set => SetProperty(ref job, value);
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

        private Position[] paths = new Position[0];
        public Position[] Paths {
            get => paths;
            set => SetProperty(ref paths, value ?? new Position[0]);
        }

        private int healthRegen;
        public int HealthRegen {
            get => healthRegen;
            set => SetProperty(ref healthRegen, value);
        }

        private AttributeDataModel[] attributes = new AttributeDataModel[0];
        public AttributeDataModel[] Attributes {
            get => attributes;
            set => SetProperty(ref attributes, value ?? new AttributeDataModel[0]);
        }
    }
}