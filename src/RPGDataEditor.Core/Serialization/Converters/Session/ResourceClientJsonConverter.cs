//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using RPGDataEditor.Connection;
//using RPGDataEditor.Core.Connection;
//using System;

//namespace RPGDataEditor.Core.Serialization
//{
//    public class ResourceClientJsonConverter : ExtendableJsonConverter<IResourceClient>
//    {
//        public override IResourceClient ReadJObject(Type objectType, JObject obj)
//        {
//            string type = obj.GetValue<string>("type");
//            IResourceClient client = CreateClient(type);
//            switch (client)
//            {
//                case FtpResourceClient ftpClient:
//                    ftpClient.Host = obj.GetValue<string>(nameof(FtpResourceClient.Host));
//                    ftpClient.RelativePath = obj.GetValue<string>(nameof(FtpResourceClient.RelativePath));
//                    ftpClient.Port = obj.GetValue<int>(nameof(FtpResourceClient.Port), 0);
//                    ftpClient.UserName = obj.GetValue<string>(nameof(FtpResourceClient.UserName));
//                    break;
//                case ExplorerResourceClient explorerClient:
//                    explorerClient.FolderPath = obj.GetValue<string>(nameof(ExplorerResourceClient.FolderPath));
//                    break;
//                case MssqlResourceClient mssqlClient:
//                    mssqlClient.ConnectionString = obj.GetValue<string>(nameof(MssqlResourceClient.ConnectionString));
//                    break;
//                default:
//                    break;
//            }
//            return client;
//        }

//        protected virtual IResourceClient CreateClient(string type) => type.ToLower() switch {
//            //"ftp" => new FtpResourceClient(),
//            //"explorer" => new ExplorerResourceClient(),
//            "mssql" => new MssqlResourceClient(),
//            _ => null,
//        };

//        public override JObject ToJObject(IResourceClient value, JsonSerializer serializer) => value switch {
//            FtpResourceClient ftpClient => new JObject() {
//                    { "type", "Ftp" },
//                    { nameof(FtpResourceClient.Host).ToLower(), ftpClient.Host },
//                    { nameof(FtpResourceClient.RelativePath).ToLower(), ftpClient.RelativePath },
//                    { nameof(FtpResourceClient.Port).ToLower(), ftpClient.Port },
//                    { nameof(FtpResourceClient.UserName).ToLower(), ftpClient.UserName }
//                },
//            ExplorerResourceClient explorerClient => new JObject() {
//                    { "type", "explorer" },
//                    { nameof(ExplorerResourceClient.FolderPath).ToLower(), explorerClient.FolderPath }
//                },
//            MssqlResourceClient mssqlClient => new JObject() {
//                    { "type", "mssql" },
//                    { nameof(MssqlResourceClient.ConnectionString).ToLower(), mssqlClient.ConnectionString }
//                },
//            _ => new JObject(value)
//        };
//    }

//}
