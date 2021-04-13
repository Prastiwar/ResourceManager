using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Providers
{
    public class DefaultRequirementProvider : DefaultModelProvider<PlayerRequirementModel>
    {
        protected override string GetTypeNameToCompare(string name) => name.Replace("Requirement", "");
    }
}