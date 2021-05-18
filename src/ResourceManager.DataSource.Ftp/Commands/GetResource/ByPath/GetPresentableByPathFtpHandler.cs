﻿using ResourceManager.DataSource.Ftp.Data;
using ResourceManager.DataSource.Local.Commands;
using ResourceManager.Services;

namespace ResourceManager.DataSource.Ftp.Commands
{
    public class GetPresentableByPathFtpHandler : GetPresentableByPathFileHandler
    {
        public GetPresentableByPathFtpHandler(IResourceDescriptorService descriptorService, IFtpFileClient client) : base(descriptorService, client) { }
    }
}