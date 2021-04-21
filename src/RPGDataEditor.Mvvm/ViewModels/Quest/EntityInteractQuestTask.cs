namespace RPGDataEditor.Mvvm.Models
{
    public class EntityInteractQuestTask : InteractQuestTask
    {
        private int entity = -1;
        public int Entity {
            get => entity;
            set => SetProperty(ref entity, value);
        }
    }
}
