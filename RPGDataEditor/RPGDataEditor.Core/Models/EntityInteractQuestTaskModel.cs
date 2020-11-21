namespace RPGDataEditor.Core.Models
{
    public class EntityInteractQuestTaskModel : InteractQuestTask
    {
        private int entity;
        public int Entity {
            get => entity;
            set => SetProperty(ref entity, value);
        }
    }
}
