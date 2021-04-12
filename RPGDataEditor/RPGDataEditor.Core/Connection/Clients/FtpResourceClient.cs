using FluentFTP;
using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Connection
{
    public class FtpResourceClient : ObservableModel, IContentResourceClient
    {
        public FtpResourceClient(IResourceToPathConverter pathConverter, IResourceToTypeConverter typeConverter)
        {
            this.pathConverter = pathConverter;
            this.typeConverter = typeConverter;
        }

        private string host = "";
        public string Host {
            get => host;
            set {
                SetProperty(ref this.host, value ?? "");
                string host = this.host;
                try
                {
                    if (host.StartsWith("ftp://"))
                    {
                        host = host[(host.IndexOf("ftp://") + "ftp://".Length)..];
                    }
                    int slashIndex = host.Replace('\\', '/').IndexOf('/');
                    if (slashIndex >= 0)
                    {
                        RelativePath = host[slashIndex..];
                        host = host.Substring(0, slashIndex + 1);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException("Value is not valid path", ex);
                }
            }
        }

        private string relativePath = "";
        public string RelativePath {
            get => relativePath;
            set {
                if (!value.StartsWith('/'))
                {
                    value = value.Insert(0, "/");
                }
                SetProperty(ref relativePath, value);
            }
        }

        private string userName = "";
        public string UserName {
            get => userName;
            set => SetProperty(ref userName, value ?? "");
        }

        private SecureString password = new SecureString();
        public SecureString Password {
            get => password;
            set => SetProperty(ref password, value ?? new SecureString());
        }

        private int port = 0;
        public int Port {
            get => port;
            set => SetProperty(ref port, value);
        }

        private FtpClient client;
        protected FtpClient Client {
            get {
                if (client == null)
                {
                    client = CreateDefaultClient();
                    return client;
                }
                if (!client.IsConnected)
                {
                    client.Port = Port;
                    client.Host = Host;
                    client.Credentials.SecurePassword = Password;
                    client.Credentials.UserName = UserName;
                    return client;
                }
                return CreateDefaultClient();
            }
        }

        private readonly IResourceToPathConverter pathConverter;

        private readonly IResourceToTypeConverter typeConverter;

        protected virtual FtpClient CreateDefaultClient() => new FtpClient() {
            Credentials = new NetworkCredential(UserName, Password),
            Host = host,
            Port = port,
            BulkListing = true,
            OnLogEvent = LogFtp
        };

        protected virtual void LogFtp(FtpTraceLevel traceLevel, string message)
        {
            if (traceLevel == FtpTraceLevel.Error)
            {
                Logger.Error(message);
            }
            else
            {
                Logger.Log(message);
            }
        }

        public async Task<bool> UpdateAsync(IIdentifiable oldResource, IIdentifiable newResource)
        {
            bool saved = await SaveJsonAsync(newResource);
            if (saved)
            {
                await DeleteAsync(oldResource);
            }
            return saved;
        }

        public Task<bool> CreateAsync(IIdentifiable resource) => SaveJsonAsync(resource);

        public async Task<bool> CreateBackupAsync(int resource, string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return false;
                }
                IIdentifiable[] jsons = await GetAllAsync(resource);
                string backupJson = JsonConvert.SerializeObject(jsons);
                return await SaveJsonAsync(backupJson, filePath);
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't make backup", ex);
            }
            return false;
        }

        public async Task<bool> DeleteAsync(IIdentifiable resource)
        {
            try
            {
                string targetPath = Path.Combine(RelativePath, pathConverter.ToRelativePath(resource));
                await Client.DeleteFileAsync(targetPath);
            }
            catch (Exception ex)
            {
                Logger.Error("Deleting file threw exception", ex);
                return false;
            }
            return true;
        }

        public Task<string> GetContentAsync(IIdentifiable resource) => GetFileContentAsync(pathConverter.ToRelativePath(resource));

        public async Task<IIdentifiable> GetAsync(IIdentifiable resource)
        {
            string json = await GetFileContentAsync(pathConverter.ToRelativePath(resource));
            return JsonConvert.DeserializeObject(json, typeConverter.GetResourceType(resource)) as IIdentifiable;
        }

        public Task<string> GetContentAsync(string location) => GetFileContentAsync(location);

        public async Task<IIdentifiable> GetAsync(Type type, string location)
        {
            string json = await GetContentAsync(location);
            return JsonConvert.DeserializeObject(json, type) as IIdentifiable;
        }

        public Task<string[]> GetAllContentAsync(int resource) => GetJsonsAsync(pathConverter.ToRelativeRoot(resource));

        public async Task<IIdentifiable[]> GetAllAsync(int resource)
        {
            string[] jsons = await GetAllContentAsync(resource);
            return jsons.Select(json => JsonConvert.DeserializeObject(json, typeConverter.GetResourceType(resource)) as IIdentifiable).ToArray();
        }

        public Task<string[]> GetAllLocationsAsync(int resource) => GetFiles(pathConverter.ToRelativeRoot(resource));

        public Task<string> GetLocationAsync(IIdentifiable resource) => Task.FromResult(pathConverter.ToRelativePath(resource));

        public Task<bool> SaveJsonAsync(IIdentifiable resource)
        {
            try
            {
                string json = JsonConvert.SerializeObject(resource);
                return SaveJsonAsync(json, pathConverter.ToRelativePath(resource));
            }
            catch (Exception ex)
            {
                Logger.Error("Saving file threw exception", ex);
                return Task.FromResult(false);
            }
        }

        public async Task<bool> SaveJsonAsync(string json, string filePath)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                byte[] fileContents = Encoding.UTF8.GetBytes(json);
                string targetPath = Path.Combine(RelativePath, filePath);
                FtpStatus status = await Client.UploadAsync(fileContents, targetPath, FtpRemoteExists.Overwrite, true);
                return status.IsSuccess();
            }
            catch (Exception ex)
            {
                Logger.Error("Saving file threw exception", ex);
                return false;
            }
        }

        public async Task<string[]> GetJsonsAsync(string relativePath)
        {
            string[] files = await GetFiles(relativePath);
            List<string> jsons = new List<string>();
            foreach (string file in files)
            {
                jsons.Add(await GetFileContentAsync(file));
            }
            return jsons.ToArray();
        }

        protected async Task<string> GetFileContentAsync(string relativePath)
        {
            string targetPath = Path.Combine(RelativePath, relativePath);
            byte[] bytes = await Client.DownloadAsync(targetPath, default);
            if (bytes == null)
            {
                Logger.Error("Couldn't download file at " + relativePath);
                return null;
            }
            string content = Encoding.UTF8.GetString(bytes);
            return content;
        }

        protected async Task<string[]> GetFiles(string relativePath)
        {
            MemoryStream stream = new MemoryStream();
            string targetPath = Path.Combine(RelativePath, relativePath);
            FtpListItem[] items = await Client.GetListingAsync(targetPath, FtpListOption.Recursive);
            return items.Where(item => item.Type == FtpFileSystemObjectType.File)
                        .Select(item => item.FullName).ToArray();
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                await Client.ConnectAsync();
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't connect to FTP", ex);
                return false;
            }
            return true;
        }

        public async Task<bool> DisconnectAsync()
        {
            try
            {
                await Client.DisconnectAsync();
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't disconnect from FTP", ex);
                return false;
            }
            return true;
        }
    }
}
