using Newtonsoft.Json;
using RPGDataEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RPGDataEditor.Core.Mvvm
{
    public class SessionContext : ObservableModel
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

        public void SaveSession(string path)
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, json);
        }

        public bool IsFtp => LocationPath.StartsWith("ftp:");

        public async Task<bool> IsValidAsync()
        {
            if (IsFtp)
            {
                try
                {
                    FtpWebRequest request = CreateFtpRequest(LocationPath, WebRequestMethods.Ftp.PrintWorkingDirectory);
                    using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                    return IsResponseSuccess(response);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return Directory.Exists(LocationPath);
            }
        }

        public async Task<bool> DeleteFileAsync(string relativePath)
        {
            if (IsFtp)
            {
                FtpWebRequest request = CreateFtpRequest(relativePath, WebRequestMethods.Ftp.DeleteFile);
                using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                return IsResponseSuccess(response);
            }
            else
            {
                try
                {
                    File.Delete(relativePath);
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
        }

        public Task<bool> SaveJsonFileAsync(string relativePath, string json)
        {
            if (IsFtp)
            {
                return SaveFtpJsonFileAsync(relativePath, json);
            }
            try
            {
                string path = Path.Combine(LocationPath, relativePath);
                new FileInfo(path).Directory.Create();
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public async Task<T[]> LoadAsync<T>(string relativePath)
        {
            string[] jsons = await LoadJsonsAsync(relativePath);
            T[] models = new T[jsons.Length];
            for (int i = 0; i < jsons.Length; i++)
            {
                models[i] = JsonConvert.DeserializeObject<T>(jsons[i]);
            }
            return models;
        }

        public Task<string[]> LoadJsonsAsync(string relativePath)
        {
            if (IsFtp)
            {
                return LoadFtpJsonsAsync(relativePath);
            }
            List<string> jsons = new List<string>();
            string directoryPath = Path.Combine(LocationPath, relativePath);
            if (Directory.Exists(directoryPath))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(directoryPath, "*.json", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    jsons.Add(File.ReadAllText(file));
                }
            }
            return Task.FromResult(jsons.ToArray());
        }

        private async Task<string[]> LoadFtpJsonsAsync(string relativePath)
        {
            string[] files = await GetFtpFiles(relativePath);
            List<string> jsons = new List<string>();
            string directoryPath = Path.Combine(LocationPath, relativePath);
            foreach (string file in files)
            {
                if (!file.EndsWith(".json"))
                {
                    continue;
                }
                string json = await ReadFtpFile(Path.Combine(directoryPath, file));
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
                return null;
            }
            using StreamReader reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }

        private async Task<string[]> GetFtpFiles(string relativePath)
        {
            FtpWebRequest request = CreateFtpRequest(relativePath, WebRequestMethods.Ftp.ListDirectory);
            using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            if (!IsResponseSuccess(response))
            {
                return new string[0];
            }
            using StreamReader reader = new StreamReader(response.GetResponseStream());
            string names = reader.ReadToEnd();
            return names.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        }

        private async Task<bool> SaveFtpJsonFileAsync(string relativePath, string json)
        {
            FtpWebRequest request = CreateFtpRequest(relativePath, WebRequestMethods.Ftp.UploadFile);

            byte[] fileContents = Encoding.UTF8.GetBytes(json);
            request.ContentLength = fileContents.Length;

            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }
            using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            return IsResponseSuccess(response);
        }

        private FtpWebRequest CreateFtpRequestRaw(string ftpPath, string method)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
            request.Method = method;
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
