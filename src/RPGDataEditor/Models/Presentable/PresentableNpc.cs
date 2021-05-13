using ResourceManager.Data;

namespace RPGDataEditor.Models
{
    public class PresentableNpc : PresentableData
    {
        public PresentableNpc() : base(typeof(Npc)) { }

        protected override void UpdateFromResource(object resource)
        {
            if (resource is Npc npc)
            {
                Id = npc.Id;
                Name = npc.Name;
            }
        }
    }
}
