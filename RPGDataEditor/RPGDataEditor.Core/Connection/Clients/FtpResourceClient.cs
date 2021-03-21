using FluentFTP;
using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
                    Client.Host = host;
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
            set {
                SetProperty(ref userName, value ?? "");
                Client.Credentials.UserName = userName;
            }
        }

        private string password = "";
        public string Password {
            get => password;
            set {
                SetProperty(ref password, value ?? "");
                Client.Credentials.Password = password;
            }
        }

        private int port = 0;
        public int Port {
            get => port;
            set {
                SetProperty(ref port, value);
                Client.Port = port;
            }
        }

        protected FtpClient Client { get; } = new FtpClient() {
            Credentials = new NetworkCredential(),
            BulkListing = true
        };

        private readonly IResourceToPathConverter pathConverter;

        private readonly IResourceToTypeConverter typeConverter;

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
                await EnsureConnectedAsync();
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

        public Task<string> GetContentAsync(IIdentifiable resource) => GetContentAsync(pathConverter.ToRelativePath(resource));

        public async Task<IIdentifiable> GetAsync(IIdentifiable resource)
        {
            string json = await GetContentAsync(pathConverter.ToRelativePath(resource));
            return JsonConvert.DeserializeObject(json, typeConverter.GetResourceType(resource)) as IIdentifiable;
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
                string targetPath = Path.Combine(RelativePath, pathConverter.ToRelativePath(resource));
                return SaveJsonAsync(json, targetPath);
            }
            catch (Exception ex)
            {
                Logger.Error("Saving file threw exception", ex);
                return Task.FromResult(false);
            }
        }

        public async Task<bool> SaveJsonAsync(string json, string targetPath)
        {
            try
            {
                await EnsureConnectedAsync();
                MemoryStream stream = new MemoryStream();
                byte[] fileContents = Encoding.UTF8.GetBytes(json);
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
            Task<string>[] tasks = files.Select(file => GetContentAsync(file)).ToArray();
            await Task.WhenAll(tasks);
            foreach (Task<string> task in tasks)
            {
                string json = await task;
                if (json != null)
                {
                    jsons.Add(json);
                }
            }
            return jsons.ToArray();
        }

        protected async Task<string> GetContentAsync(string relativePath)
        {
            await EnsureConnectedAsync();
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
            await EnsureConnectedAsync();
            MemoryStream stream = new MemoryStream();
            string targetPath = Path.Combine(RelativePath, relativePath);
            FtpListItem[] items = await Client.GetListingAsync(targetPath, FtpListOption.Recursive);
            return items.Where(item => item.Type == FtpFileSystemObjectType.File)
                        .Select(item => item.FullName).ToArray();
        }

        protected async Task EnsureConnectedAsync()
        {
            if (!Client.IsConnected)
            {
                await Client.ConnectAsync();
            }
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
