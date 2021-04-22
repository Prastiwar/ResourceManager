namespace ResourceManager.Commands
{
    public class UpdateResourceResults : CommandResults
    {
        public UpdateResourceResults(bool isSuccess) => IsSuccess = isSuccess;
    }
}
