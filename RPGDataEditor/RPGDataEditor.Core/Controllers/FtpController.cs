using FluentFTP;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public class FtpController : ObservableModel, IJsonFilesController
    {
        private string locationPath = "";
        public string LocationPath {
            get => locationPath;
            set {
                SetProperty(ref locationPath, value ?? "");
                string host = locationPath;
                try
                {
                    if (host.StartsWith("ftp://"))
                    {
                        host = host[(host.IndexOf("ftp://") + "ftp://".Length)..];
                    }
                    int slashIndex = host.Replace('\\', '/').IndexOf('/');
                    if (slashIndex >= 0)
                    {
                        RelativeLocationPath = host[slashIndex..];
                        host = host.Substring(0, slashIndex + 1);
                    }
                    client.Host = host;
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException("Value is not valid path", ex);
                }
            }
        }

        public string relativeLocationPath;
        public string RelativeLocationPath {
            get => relativeLocationPath;
            protected set {
                if (!value.StartsWith('/'))
                {
                    value = value.Insert(0, "/");
                }
                SetProperty(ref relativeLocationPath, value);
            }
        }

        private string userName = "";
        public string UserName {
            get => userName;
            set {
                SetProperty(ref userName, value ?? "");
                client.Credentials.UserName = userName;
            }
        }

        private string password = "";
        public string Password {
            get => password;
            set {
                SetProperty(ref password, value ?? "");
                client.Credentials.Password = password;
            }
        }

        private readonly FtpClient client = new FtpClient() {
            Credentials = new NetworkCredential(),
            BulkListing = true
        };

        public async Task<bool> IsValidAsync()
        {
            try
            {
                await client.ConnectAsync();
                await client.DisconnectAsync();
            }
            catch (Exception ex)
            {
                Logger.Error("FTP validation error", ex);
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteFileAsync(string relativePath)
        {
            try
            {
                await EnsureConnectedAsync();
                string targetPath = Path.Combine(RelativeLocationPath, relativePath);
                await client.DeleteFileAsync(targetPath);
            }
            catch (Exception ex)
            {
                Logger.Error("Deleting file threw exception", ex);
                return false;
            }
            return true;
        }

        public async Task<bool> SaveJsonAsync(string relativePath, string json)
        {
            try
            {
                await EnsureConnectedAsync();
                MemoryStream stream = new MemoryStream();
                byte[] fileContents = Encoding.UTF8.GetBytes(json);
                string targetPath = Path.Combine(RelativeLocationPath, relativePath);
                FtpStatus status = await client.UploadAsync(fileContents, targetPath, FtpRemoteExists.Overwrite, true);
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
            string[] files = await GetJsonFiles(relativePath);
            List<string> jsons = new List<string>();
            Task<string>[] tasks = files.Select(file => GetJsonAsync(file)).ToArray();
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

        public async Task<string> GetJsonAsync(string relativePath)
        {
            await EnsureConnectedAsync();
            string targetPath = Path.Combine(RelativeLocationPath, relativePath);
            byte[] bytes = await client.DownloadAsync(targetPath, default);
            if (bytes == null)
            {
                Logger.Error("Couldn't download file at " + relativePath);
                return null;
            }
            string content = Encoding.UTF8.GetString(bytes);
            return content;
        }

        public async Task<string[]> GetJsonFiles(string relativePath)
        {
            await EnsureConnectedAsync();
            MemoryStream stream = new MemoryStream();
            string targetPath = Path.Combine(RelativeLocationPath, relativePath);
            FtpListItem[] items = await client.GetListingAsync(targetPath, FtpListOption.Recursive);
            return items.Where(item => item.Type == FtpFileSystemObjectType.File)
                        .Select(item => item.FullName).ToArray();
        }

        protected async Task EnsureConnectedAsync()
        {
            if (!client.IsConnected)
            {
                await client.ConnectAsync();
            }
        }
    }
}
