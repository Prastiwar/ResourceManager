using RPGDataEditor.Models;

namespace RPGDataEditor.Providers
{
    public class DefaultRequirementProvider : DefaultModelProvider<PlayerRequirementModel>
    {
        protected override string GetTypeNameToCompare(string name) => name.Replace("Requirement", "");
    }
}