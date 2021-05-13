//    public class FtpResourceClient : IContentResourceClient
//    {
//        public FtpResourceClient(ISerializer serializer) => Serializer = serializer;

//        private string host = "";
//        public string Host {
//            get => host;
//            set {
//                string host = value ?? "";
//                try
//                {
//                    if (host.StartsWith("ftp://"))
//                    {
//                        host = host[(host.IndexOf("ftp://") + "ftp://".Length)..];
//                    }
//                    int slashIndex = host.Replace('\\', '/').IndexOf('/');
//                    if (slashIndex >= 0)
//                    {
//                        RelativePath = host[slashIndex..];
//                        host = host.Substring(0, slashIndex);
//                    }
//                    this.host = host;
//                }
//                catch (Exception ex)
//                {
//                    throw new InvalidDataException("Value is not valid path", ex);
//                }
//            }
//        }

//        private string relativePath = "";
//        public string RelativePath {
//            get => relativePath;
//            set {
//                if (!value.StartsWith('/'))
//                {
//                    value = value.Insert(0, "/");
//                }
//                relativePath = value;
//            }
//        }

//        private FtpClient client;
//        protected FtpClient Client {
//            get {
//                if (client == null)
//                {
//                    client = CreateDefaultClient();
//                    return client;
//                }
//                if (!client.IsConnected)
//                {
//                    client.Port = Port;
//                    client.Host = Host;
//                    client.Credentials.SecurePassword = Password;
//                    client.Credentials.UserName = UserName;
//                    return client;
//                }
//                return CreateDefaultClient();
//            }
//        }

//        public IResourceDescriptorCollection Descriptors { get; set; }

//        protected ISerializer Serializer { get; }

//        protected virtual FtpClient CreateDefaultClient() => new FtpClient() {
//            Credentials = new NetworkCredential(UserName, Password),
//            Host = Host,
//            Port = Port,
//            BulkListing = true,
//            OnLogEvent = LogFtp
//        };

//        protected virtual void LogFtp(FtpTraceLevel traceLevel, string message)
//        {
//            if (traceLevel == FtpTraceLevel.Error)
//            {
//                Logger.Error(message);
//            }
//            else
//            {
//                Logger.Log(message);
//            }
//        }

//        public async Task<bool> UpdateAsync(object oldResource, object newResource)
//        {
//            bool saved = await SaveAsync(newResource);
//            if (saved)
//            {
//                await DeleteAsync(oldResource);
//            }
//            return saved;
//        }

//        public Task<bool> CreateAsync(object resource) => SaveAsync(resource);

//        public async Task<bool> CreateBackupAsync(ResourceDescriptor descriptor, string filePath)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(filePath))
//                {
//                    return false;
//                }
//                object[] objects = await GetAllAsync(descriptor);
//                string backup = Serializer.Serialize(objects);
//                return await SaveAsync(backup, filePath);
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("Couldn't make backup", ex);
//            }
//            return false;
//        }

//        public async Task<bool> DeleteAsync(object resource)
//        {
//            try
//            {
//                string targetPath = Path.Combine(RelativePath, Descriptors.Describe(resource).GetRelativeFullPath(resource));
//                await Client.DeleteFileAsync(targetPath);
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("Deleting file threw exception", ex);
//                return false;
//            }
//            return true;
//        }

//        public Task<string> GetContentAsync(object resource) => GetFileContentAsync(Descriptors.Describe(resource).GetRelativeFullPath(resource));

//        public async Task<object> GetAsync(object resource)
//        {
//            ResourceDescriptor descriptor = Descriptors.Describe(resource);
//            string content = await GetFileContentAsync(descriptor.GetRelativeFullPath(resource));
//            return Serializer.Deserialize(content, descriptor.Type);
//        }

//        public Task<string> GetContentAsync(string location) => GetFileContentAsync(location);

//        public async Task<object> GetAsync(Type type, string location)
//        {
//            string content = await GetContentAsync(location);
//            return Serializer.Deserialize(content, type);
//        }

//        public Task<string[]> GetAllContentAsync(ResourceDescriptor descriptor) => GetContentsAsync(descriptor.RelativeRootPath);

//        public async Task<object[]> GetAllAsync(ResourceDescriptor descriptor)
//        {
//            string[] contents = await GetAllContentAsync(descriptor);
//            return contents.Select(content => Serializer.Deserialize(content, descriptor.Type)).ToArray();
//        }

//        public Task<string[]> GetAllLocationsAsync(ResourceDescriptor descriptor) => GetFiles(descriptor.RelativeRootPath);

//        public Task<string> GetLocationAsync(object resource) => Task.FromResult(Descriptors.Describe(resource)?.GetRelativeFullPath(resource));

//        public Task<bool> SaveAsync(object resource)
//        {
//            try
//            {
//                string content = Serializer.Serialize(resource);
//                return SaveAsync(content, Descriptors.Describe(resource).GetRelativeFullPath(resource));
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("Saving file threw exception", ex);
//                return Task.FromResult(false);
//            }
//        }

//        public async Task<bool> SaveAsync(string content, string filePath)
//        {
//            try
//            {
//                MemoryStream stream = new MemoryStream();
//                byte[] fileContents = Encoding.UTF8.GetBytes(content);
//                string targetPath = Path.Combine(RelativePath, filePath);
//                FtpStatus status = await Client.UploadAsync(fileContents, targetPath, FtpRemoteExists.Overwrite, true);
//                return status.IsSuccess();
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("Saving file threw exception", ex);
//                return false;
//            }
//        }

//        public async Task<string[]> GetContentsAsync(string relativePath)
//        {
//            string[] files = await GetFiles(relativePath);
//            List<string> contents = new List<string>();
//            foreach (string file in files)
//            {
//                contents.Add(await GetFileContentAsync(file));
//            }
//            return contents.ToArray();
//        }

//        protected async Task<string> GetFileContentAsync(string relativePath)
//        {
//            string targetPath = Path.Combine(RelativePath, relativePath);
//            byte[] bytes = await Client.DownloadAsync(targetPath, default);
//            if (bytes == null)
//            {
//                Logger.Error("Couldn't download file at " + relativePath);
//                return null;
//            }
//            string content = Encoding.UTF8.GetString(bytes);
//            return content;
//        }

//        protected async Task<string[]> GetFiles(string relativePath)
//        {
//            MemoryStream stream = new MemoryStream();
//            string targetPath = Path.Combine(RelativePath, relativePath);
//            FtpListItem[] items = await Client.GetListingAsync(targetPath, FtpListOption.Recursive);
//            return items.Where(item => item.Type == FtpFileSystemObjectType.File)
//                        .Select(item => item.FullName).ToArray();
//        }

//        public async Task<bool> ConnectAsync()
//        {
//            try
//            {
//                await Client.ConnectAsync();
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("Couldn't connect to FTP", ex);
//                return false;
//            }
//            return true;
//        }

//        public async Task<bool> DisconnectAsync()
//        {
//            try
//            {
//                await Client.DisconnectAsync();
//            }
//            catch (Exception ex)
//            {
//                Logger.Error("Couldn't disconnect from FTP", ex);
//                return false;
//            }
//            return true;
//        }