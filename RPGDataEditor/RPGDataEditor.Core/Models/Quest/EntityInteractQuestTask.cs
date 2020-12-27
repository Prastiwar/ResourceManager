namespace RPGDataEditor.Core.Models
{
    public class EntityInteractQuestTask : InteractQuestTask
    {
        private int entity;
        public int Entity {
            get => entity;
            set => SetProperty(ref entity, value);
        }
    }
}
