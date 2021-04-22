using ResourceManager.Commands;

namespace RPGDataEditor.Commands
{
    public class RemoveCategoryResults : CommandResults
    {
        public RemoveCategoryResults(bool isSuccess) => IsSuccess = isSuccess;
    }
}
