using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core.Providers
{
    public interface IRequirementProvider
    {
        PlayerRequirementModel CreateRequirement(string name);
    }
}