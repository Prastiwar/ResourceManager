﻿namespace RpgDataEditor.Models
{
    public class QuestRequirement : Requirement
    {
        public object QuestId { get; set; }

        public QuestStage Stage { get; set; }
    }
}
