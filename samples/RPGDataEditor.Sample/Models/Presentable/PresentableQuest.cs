using ResourceManager.Data;

namespace RPGDataEditor.Sample.Models
{
    public class PresentableQuest : PresentableCategoryData
    {
        public PresentableQuest() : base(typeof(Quest)) { }

        protected override void UpdateFromResource(object resource)
        {
            if (resource is Quest quest)
            {
                Id = quest.Id;
                Name = quest.Title;
            }
        }
    }
}
