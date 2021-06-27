namespace ResourceManager.Mvvm
{
    public class EditorResults
    {
        public EditorResults(object model, bool success)
        {
            Model = model;
            Success = success;
        }

        public object Model { get; }
        public bool Success { get; set; }
    }
}
