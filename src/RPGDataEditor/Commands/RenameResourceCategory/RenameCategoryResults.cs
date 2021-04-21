using ResourceManager.Commands;

namespace RPGDataEditor.Commands
{
    public class RenameCategoryResults : CommandResults
    {
        public RenameCategoryResults(bool isSuccess) => IsSuccess = isSuccess;
    }
}
