using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Minecraft.Providers
{
    public class RequirementProvider : McModelProvider<PlayerRequirementModel>
    {
        protected override string GetTypeNameToCompare(string name) => name.Replace("Requirement", "");
    }
}