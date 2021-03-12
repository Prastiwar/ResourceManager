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
            //if (files.Length >= 100)
            //{
            //    TransformBlock<string, string> getCustomerBlock = new TransformBlock<string, string>(async file => await ReadFtpFile(file),
            //        new ExecutionDataflowBlockOptions {
            //            MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
            //        });
            //    ActionBlock<string> writeCustomerBlock = new ActionBlock<string>(json => {
            //        if (json != null)
            //        {
            //            jsons.Add(json);
            //        }
            //    });
            //    getCustomerBlock.LinkTo(writeCustomerBlock, new DataflowLinkOptions {
            //        PropagateCompletion = true
            //    });
            //    foreach (string file in files)
            //    {
            //        getCustomerBlock.Post(file);
            //    }
            //    getCustomerBlock.Complete();
            //    await writeCustomerBlock.Completion;
            //}
            //else
            //{
            Task<string>[] tasks = files.Select(file => ReadFtpFile(file)).ToArray();
            await Task.WhenAll(tasks);
            foreach (Task<string> task in tasks)
            {
                string json = await task;
                if (json != null)
                {
                    jsons.Add(json);
                }
            }
            //}
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
            FtpWebRequest request = CreateFtpRequest(relativePath, WebRequestMethods.Ftp.ListDirectoryDetails);
            using FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            if (!IsResponseSuccess(response))
            {
                Logger.ErrorFtp(response);
                return new string[0];
            }
            using StreamReader reader = new StreamReader(response.GetResponseStream());
            string names = reader.ReadToEnd();
            List<string> files = names.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> fileNames = new List<string>(files.Count);
            foreach (string file in files)
            {
                bool isDirectory = file[0] == 'd';
                string fileName = file.Split(new char[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries)[8];
                if (isDirectory)
                {
                    string[] subFiles = await GetJsonFiles(Path.Combine(relativePath, fileName));
                    fileNames.AddRange(subFiles);
                } 
                else if(!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    string directoryPath = Path.Combine(LocationPath, relativePath);
                    string filePath = Path.Combine(directoryPath, fileName);
                    fileNames.Add(filePath);
                }
            }
            return fileNames.ToArray();
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
