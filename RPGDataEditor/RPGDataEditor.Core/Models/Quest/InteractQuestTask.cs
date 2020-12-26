﻿namespace RPGDataEditor.Core.Models
{
    public class InteractQuestTask : QuestTask
    {
        private bool completed;
        public bool Completed {
            get => completed;
            set => SetProperty(ref completed, value);
        }
    }
}
