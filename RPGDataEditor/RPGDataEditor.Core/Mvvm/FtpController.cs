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
            set => SetProperty(ref locationPath, value ?? "");
        }

        private string ftpUserName = "";
        public string FtpUserName {
            get => ftpUserName;
            set => SetProperty(ref ftpUserName, value ?? "");
        }

        private string ftpPassword = "";
        public string FtpPassword {
            get => ftpPassword;
            set => SetProperty(ref ftpPassword, value ?? "");
        }

        public async Task<bool> IsValidAsync()
        {
            try
            {
                FtpWebRequest request = CreateFtpRequest(LocationPath, WebRequestMethods.Ftp.PrintWorkingDirectory);
                using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                if (!IsResponseSuccess(response))
                {
                    Logger.ErrorFtp(response);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Session Validation error", ex);
                return false;
            }
        }

        public async Task<bool> DeleteFileAsync(string relativePath)
        {
            FtpWebRequest request = CreateFtpRequest(relativePath, WebRequestMethods.Ftp.DeleteFile);
            using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
            {
                if (!IsResponseSuccess(response))
                {
                    Logger.ErrorFtp(response);
                    return false;
                }
                return true;
            }
        }

        public async Task<bool> SaveJsonAsync(string relativePath, string json)
        {
            FtpWebRequest request = CreateFtpRequest(relativePath, WebRequestMethods.Ftp.UploadFile);

            byte[] fileContents = Encoding.UTF8.GetBytes(json);
            request.ContentLength = fileContents.Length;

            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }
            using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
            {
                if (IsResponseSuccess(response))
                {
                    return true;
                }
                Logger.ErrorFtp(response);
                return false;
            }
        }

        public Task<string> GetJsonAsync(string relativePath) => ReadFtpFile(Path.Combine(LocationPath, relativePath));

        public async Task<string[]> GetJsonsAsync(string relativePath)
        {
            string[] files = await GetJsonFiles(relativePath);
            List<string> jsons = new List<string>();
            foreach (string file in files)
            {
                string json = await ReadFtpFile(file);
                if (json != null)
                {
                    jsons.Add(json);
                }
            }
            return jsons.ToArray();
        }

        private async Task<string> ReadFtpFile(string filePath)
        {
            FtpWebRequest request = CreateFtpRequestRaw(filePath, WebRequestMethods.Ftp.DownloadFile);
            using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            if (!IsResponseSuccess(response))
            {
                Logger.ErrorFtp(response);
                return null;
            }
            using StreamReader reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }

        public async Task<string[]> GetJsonFiles(string relativePath)
        {
            FtpWebRequest request = CreateFtpRequest(relativePath, WebRequestMethods.Ftp.ListDirectory);
            using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            if (!IsResponseSuccess(response))
            {
                Logger.ErrorFtp(response);
                return new string[0];
            }
            using StreamReader reader = new StreamReader(response.GetResponseStream());
            string names = reader.ReadToEnd();
            List<string> files = names.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            files.RemoveAll(file => !file.EndsWith(".json"));
            string directoryPath = Path.Combine(LocationPath, relativePath);
            return files.Select(file => Path.Combine(directoryPath, file)).ToArray();
        }

        private FtpWebRequest CreateFtpRequestRaw(string ftpPath, string method)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
            request.Method = method;
            request.KeepAlive = false;
            return request;
        }

        private FtpWebRequest CreateFtpRequest(string relativePath, string method) => CreateFtpRequestRaw(Path.Combine(LocationPath, relativePath), method);

        private bool IsResponseSuccess(FtpWebResponse response)
        {
            int statusCode = (int)response.StatusCode;
            return statusCode >= 200 && statusCode < 300 || statusCode == 125 || statusCode == 150;
        }
    }
}
