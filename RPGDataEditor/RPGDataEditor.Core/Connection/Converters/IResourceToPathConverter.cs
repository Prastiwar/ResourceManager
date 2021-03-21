namespace RPGDataEditor.Core.Connection
{
    public interface IResourceToPathConverter
    {
        string ToRelativeRoot(int resource);

        string ToRelativeRoot(IIdentifiable resource);

        string ToRelativePath(IIdentifiable resource);
    }
}
