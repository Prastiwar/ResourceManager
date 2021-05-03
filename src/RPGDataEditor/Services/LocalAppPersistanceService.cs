using System.IO;
using System.Threading.Tasks;

namespace RPGDataEditor.Services
{
    public class LocalAppPersistanceService : IAppPersistanceService
    {
        public LocalAppPersistanceService(ISerializer serializer) => Serializer = serializer;

        public string FolderPath { get; set; }

        protected ISerializer Serializer { get; }

        public Task SaveAsync(string name, object obj)
        {
            string text = Serializer.Serialize(obj);
            lock (FolderPath)
            {
                File.AppendAllText(Path.Combine(FolderPath, name, ".json"), text);
            }
            return Task.CompletedTask;
        }
    }
}
