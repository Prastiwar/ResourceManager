﻿using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Serialization
{
    public class NpcJobJsonConverter : AbstractClassJsonConverter<NpcJobModel>
    {
        public NpcJobJsonConverter() : base("RPGDataEditor.Core.Models") { }

        protected override string Suffix => "NpcJobModel";
    }
}
