using System;
using System.ServiceProcess;

namespace RpgDataEditor.Tests
{
    public class FtpTestServer
    {
        public FtpTestServer(string rootDirectory) => RootDirectory = rootDirectory;

        public string RootDirectory { get; }

        public void Start()
        {
            ServiceController controller = GetServerServiceAsync();
            if (controller.Status != ServiceControllerStatus.Running)
            {
                throw new InvalidProgramException("ResourceManagerFtpTestServer service is not running. Cannot run test.");
            }
        }

        public void Stop()
        {
            ServiceController controller = GetServerServiceAsync();
            controller.Stop();
        }

        protected ServiceController GetServerServiceAsync()
        {
            try
            {
                return new ServiceController("ResourceManagerFtpTestServer");
            }
            catch (Exception)
            {
                throw new InvalidProgramException("Use install-service.bat (run as administrator) to install ftp service for this test. Set ${DirectoryPath} in `FileZilla Server.xml` to " + RootDirectory);
            }
        }
    }
}
