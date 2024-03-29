﻿using ResourceManager.Data;
using System.Collections.Generic;

namespace RpgDataEditor.Models
{
    public class Npc : IIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Position Position { get; set; }

        public TalkData TalkData { get; set; }

        public NpcJob Job { get; set; }

        public IList<AttributeData> Attributes { get; set; }

        object IIdentifiable.Id {
            get => Id;
            set => Id = (int)System.Convert.ToInt64(value);
        }
    }
}
