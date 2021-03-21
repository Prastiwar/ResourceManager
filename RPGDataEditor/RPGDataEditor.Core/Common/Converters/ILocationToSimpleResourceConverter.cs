using RPGDataEditor.Core.Models;

namespace RPGDataEditor.Core
{
    public interface ILocationToSimpleResourceConverter
    {
        SimpleIdentifiableData CreateSimpleData(string location);
    }
}