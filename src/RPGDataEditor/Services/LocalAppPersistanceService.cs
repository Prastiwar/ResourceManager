using ResourceManager.Data;
using System.IO;
using System.Threading.Tasks;

namespace RPGDataEditor.Services
{
    public class LocalAppPersistanceService : IAppPersistanceService
    {
        public LocalAppPersistanceService(ITextSerializer serializer) => Serializer = serializer;

        public string FolderPath { get; set; }

        protected ITextSerializer Serializer { get; }

        public void Save(string name, object obj)
        {
            string text = Serializer.Serialize(obj);
            lock (FolderPath)
            {
                File.WriteAllText(Path.Combine(FolderPath, name + ".json"), text);
            }
        }

        public Task SaveAsync(string name, object obj)
        {
            string text = Serializer.Serialize(obj);
            lock (FolderPath)
            {
                return File.WriteAllTextAsync(Path.Combine(FolderPath, name + ".json"), text);
            }
        }
    }
}
