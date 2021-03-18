using RPGDataEditor.Core.Providers;
using RPGDataEditor.Minecraft.Models;

namespace RPGDataEditor.Minecraft
{
    public class McRequirementProvider : DefaultRequirementProvider
    {
        public override Core.Models.PlayerRequirementModel CreateRequirement(string name)
        {
            Core.Models.PlayerRequirementModel requirement = base.CreateRequirement(name);
            if (requirement is Core.Models.ItemRequirement)
            {
                return new ItemRequirement();
            }
            return requirement;
        }
    }
}