using RPGDataEditor.Models;

namespace RPGDataEditor.Providers
{
    public class DefaultRequirementProvider : DefaultImplementationProvider<Requirement>
    {
        protected override string GetTypeNameToCompare(string name) => name.Replace("Requirement", "");
    }
}