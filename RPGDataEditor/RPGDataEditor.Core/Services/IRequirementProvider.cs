using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core
{
    public interface IRequirementProvider
    {
        PlayerRequirementModel CreateRequirement(string name);
    }
}