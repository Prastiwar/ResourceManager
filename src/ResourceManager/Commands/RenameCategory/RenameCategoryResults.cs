using ResourceManager.Commands;

namespace ResourceManager.Commands
{
    public class RenameCategoryResults : CommandResults
    {
        public RenameCategoryResults(bool isSuccess) => IsSuccess = isSuccess;
    }
}
