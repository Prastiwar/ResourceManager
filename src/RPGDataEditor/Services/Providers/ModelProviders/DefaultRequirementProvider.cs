using RPGDataEditor.Models;

namespace RPGDataEditor.Providers
{
    public class DefaultRequirementProvider : DefaultModelProvider<Requirement>
    {
        protected override string GetTypeNameToCompare(string name) => name.Replace("Requirement", "");
    }
}