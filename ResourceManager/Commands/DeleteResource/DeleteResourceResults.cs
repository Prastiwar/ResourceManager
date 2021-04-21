namespace ResourceManager.Commands
{
    public class DeleteResourceResults
    {
        public DeleteResourceResults(bool isSuccess) => IsSuccess = isSuccess;

        public bool IsSuccess { get; }
    }
}
