using ResourceManager.Data;

namespace RPGDataEditor.Models
{
    public class PresentableDialogue : PresentableCategoryData
    {
        public PresentableDialogue() : base(typeof(Dialogue)) { }

        protected override void UpdateFromResource(object resource)
        {
            if (resource is Dialogue dialogue)
            {
                Id = dialogue.Id;
                Name = dialogue.Title;
            }
        }
    }
}
