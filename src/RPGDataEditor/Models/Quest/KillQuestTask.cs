﻿namespace RPGDataEditor.Models
{
    public class KillQuestTask : IQuestTask
    {
        public object TargetId { get; set; }

        public int Amount { get; set; }

        public int Counter { get; set; }
    }
}
