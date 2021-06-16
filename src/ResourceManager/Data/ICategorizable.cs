namespace ResourceManager.Data
{
    public interface ICategorizable : IIdentifiable
    {
        string Category { get; set; }
    }
}
