using ResourceManager.Commands;

namespace ResourceManager.Commands
{
    public class RemoveCategoryResults : CommandResults
    {
        public RemoveCategoryResults(bool isSuccess) => IsSuccess = isSuccess;
    }
}
