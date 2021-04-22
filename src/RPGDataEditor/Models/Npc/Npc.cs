﻿using System.Collections.Generic;

namespace RPGDataEditor.Models
{
    public class NpcDataModel : IIdentifiable
    {
        public object Id { get; set; }

        public string Name { get; set; }

        public Position Position { get; set; }

        public TalkData TalkData { get; set; }

        public NpcJob Job { get; set; }

        public IList<AttributeData> Attributes { get; private set; }
    }
}