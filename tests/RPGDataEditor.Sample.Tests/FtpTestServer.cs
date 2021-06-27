using System;
using System.Diagnostics;
using System.IO;

namespace RPGDataEditor.Sample.Tests
{
    public class FtpTestServer : IDisposable
    {
        private Process ftpProcess;

        public FtpTestServer(string rootDirectory)
        {
            string serverConfig = File.ReadAllText("./Fixtures/Ftp-Server/FileZilla Server-Template.xml");
            serverConfig = serverConfig.Replace("${DirectoryPath}", rootDirectory);
            File.WriteAllText("./Fixtures/Ftp-Server/FileZilla Server.xml", serverConfig);

            string serverPath = Path.GetFullPath("./Fixtures/Ftp-Server/FileZilla Server.exe");
            ProcessStartInfo psInfo = new ProcessStartInfo {
                FileName = serverPath,
                Arguments = "/compat /start",
                WindowStyle = ProcessWindowStyle.Hidden
            };
            ftpProcess = Process.Start(psInfo);
        }

        public void Dispose()
        {
            ftpProcess.Kill();
            ftpProcess.WaitForExit();

            string serverPath = Path.GetFullPath("./Fixtures/Ftp-Server/FileZilla Server.exe");
            ProcessStartInfo psInfo = new ProcessStartInfo {
                FileName = serverPath,
                Arguments = "/compat /stop",
                WindowStyle = ProcessWindowStyle.Hidden
            };
            ftpProcess = Process.Start(psInfo);
            ftpProcess.Kill();
            ftpProcess.WaitForExit();
        }
    }
}
