using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Core.Connection;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class FileClientJsonConverter : ExtendableJsonConverter<IFileClient>
    {
        public override IFileClient ReadJObject(Type objectType, JObject obj)
        {
            string type = obj.GetValue<string>("type");
            IFileClient client = CreateClient(type);
            switch (client)
            {
                case FtpFileClient ftpClient:
                    ftpClient.Host = obj.GetValue<string>(nameof(FtpFileClient.Host));
                    ftpClient.RelativePath = obj.GetValue<string>(nameof(FtpFileClient.RelativePath));
                    ftpClient.Port = obj.GetValue<int>(nameof(FtpFileClient.Port), 0);
                    ftpClient.UserName = obj.GetValue<string>(nameof(FtpFileClient.UserName));
                    break;
                case LocalFileClient LocalClient:
                    LocalClient.FolderPath = obj.GetValue<string>(nameof(LocalFileClient.FolderPath));
                    break;
                default:
                    break;
            }
            return client;
        }

        protected virtual IFileClient CreateClient(string type) => type.ToLower() switch {
            "ftp" => new FtpFileClient(),
            "local" => new LocalFileClient(),
            _ => null,
        };

        public override JObject ToJObject(IFileClient value, JsonSerializer serializer) => value switch {
            FtpFileClient ftpClient => new JObject() {
                    { "type", "ftp" },
                    { nameof(FtpFileClient.Host).ToLower(), ftpClient.Host },
                    { nameof(FtpFileClient.RelativePath).ToLower(), ftpClient.RelativePath },
                    { nameof(FtpFileClient.Port).ToLower(), ftpClient.Port },
                    { nameof(FtpFileClient.UserName).ToLower(), ftpClient.UserName }
                },
            LocalFileClient LocalClient => new JObject() {
                    { "type", "local" },
                    { nameof(LocalFileClient.FolderPath).ToLower(), LocalClient.FolderPath }
                },
            _ => new JObject(value)
        };
    }

}
