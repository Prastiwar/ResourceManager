namespace RPGDataEditor.Mvvm.Models
{
    public class QuestRequirement : PlayerRequirementModel
    {
        private int questId = -1;
        public int QuestId {
            get => questId;
            set => SetProperty(ref questId, value);
        }

        private QuestStage stage;
        public QuestStage Stage {
            get => stage;
            set => SetProperty(ref stage, value);
        }
    }
}
