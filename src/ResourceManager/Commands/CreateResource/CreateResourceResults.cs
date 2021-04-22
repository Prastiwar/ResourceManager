namespace ResourceManager.Commands
{
    public class CreateResourceResults : CommandResults
    {
        public CreateResourceResults(bool isSuccess) => IsSuccess = isSuccess;
    }
}
