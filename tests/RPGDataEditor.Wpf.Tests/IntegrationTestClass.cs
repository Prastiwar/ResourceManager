using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceManager;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.DataSource.Sql.Data;
using ResourceManager.Services;
using RPGDataEditor.Core.Services;
using RPGDataEditor.Wpf.Converters;
using System;
using System.IO;
using System.Threading;

namespace RPGDataEditor.Wpf.Tests
{
    public class IntegrationTestClass
    {
        public IntegrationTestClass() => ServiceProvider = CreateIntegratedServices();

        protected IServiceProvider ServiceProvider { get; }

        /// <summary> https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo?redirectedfrom=MSDN&view=net-5.0 </summary>
        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private void InitializeFixture(string fixtureName)
        {
            string path = $"./Fixtures/{fixtureName}";
            string tempPath = $"./Fixtures/{fixtureName}-temp";
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
            DirectoryInfo targetPath = Directory.CreateDirectory(tempPath);
            CopyAll(new DirectoryInfo(path), targetPath);
        }

        protected IDataSource GetIntegratedLocalDataSource()
        {
            IDataSource dataSource = ServiceProvider.GetRequiredService<IDataSource>();
            if (dataSource is IConfigurableDataSource configurable)
            {
                IConfigurationSection dataSourceConfiguration = ServiceProvider.GetRequiredService<IConfiguration>().SetAndGetDataSource("Local");
                LocalProxyConfiguration proxy = new LocalProxyConfiguration(dataSourceConfiguration) {
                    FolderPath = "./Fixtures/Local-temp"
                };
                InitializeFixture("Local");
                configurable.Configure(dataSourceConfiguration);
            }
            return dataSource;
        }

        protected IDataSource GetIntegratedFtpDataSource(CancellationToken token)
        {
            IDataSource dataSource = ServiceProvider.GetRequiredService<IDataSource>();
            if (dataSource is IConfigurableDataSource configurable)
            {
                IConfigurationSection dataSourceConfiguration = ServiceProvider.GetRequiredService<IConfiguration>().SetAndGetDataSource("Ftp");
                FtpProxyConfiguration proxy = new FtpProxyConfiguration(dataSourceConfiguration) {
                    Host = "localhost",
                    UserName = "testerro",
                    Password = "123456".ToSecure()
                };
                string relativePath = "Fixtures/Ftp-temp";
                string rootPath = Path.GetFullPath(relativePath);
                InitializeFixture("Ftp");
                FtpTestServer server = new FtpTestServer(rootPath);
                token.Register(server.Dispose);
                configurable.Configure(dataSourceConfiguration);
            }
            return dataSource;
        }

        protected IDataSource GetIntegratedSqlDataSource()
        {
            IDataSource dataSource = ServiceProvider.GetRequiredService<IDataSource>();
            if (dataSource is IConfigurableDataSource configurable)
            {
                IConfigurationSection dataSourceConfiguration = ServiceProvider.GetRequiredService<IConfiguration>().SetAndGetDataSource("Sql");
                SqlProxyConfiguration proxy = new SqlProxyConfiguration(dataSourceConfiguration);
                string relativePath = "./Fixtures/Sql/Database.sqlite";
                string databaseFilePath = Path.GetFullPath(relativePath);
                proxy.ConnectionString = $"Data Source = {databaseFilePath}; Version = 3;";
                InitializeFixture("Sql");
                configurable.Configure(dataSourceConfiguration);
            }
            return dataSource;
        }

        protected virtual IServiceProvider CreateIntegratedServices()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddConfiguration(builder => builder.AddInMemoryCollection());
            NewtonsoftSerializer serializer = new NewtonsoftSerializer();
            services.AddDataSourceConfiguration(builder => {
                builder.RegisterResourceTypes(typeof(Models.Quest), typeof(Models.Dialogue), typeof(Models.Npc));

                ResourceDescriptorService fileDescriptorService = new ResourceDescriptorService();
                IResourceDescriptor fileQuestDescriptor = new LocationResourceDescriptor(typeof(Models.Quest), "/quests", "/{category}/{id}_{title}.json");
                IResourceDescriptor fileDialogueDescriptor = new LocationResourceDescriptor(typeof(Models.Dialogue), "/dialogues", "/{category}/{id}_{title}.json");
                IResourceDescriptor fileNpcDescriptor = new LocationResourceDescriptor(typeof(Models.Npc), "/npcs", "/{id}_{name}.json");
                fileDescriptorService.Register<Models.Quest>(fileQuestDescriptor);
                fileDescriptorService.Register<Models.Dialogue>(fileDialogueDescriptor);
                fileDescriptorService.Register<Models.Npc>(fileNpcDescriptor);

                builder.AddLocalDataSource(o => {
                    o.DescriptorService = fileDescriptorService;
                    o.Serializer = serializer;
                });

                builder.AddFtpDataSource(o => {
                    o.DescriptorService = fileDescriptorService;
                    o.Serializer = serializer;
                });

                builder.AddSqlDataSource(o => {
                    IResourceDescriptor sqlQuestDescriptor = new SqlLocationResourceDescriptor(typeof(Models.Quest), "quests", ".{id}");
                    IResourceDescriptor sqlDialogueDescriptor = new SqlLocationResourceDescriptor(typeof(Models.Dialogue), "dialogues", ".{id}");
                    IResourceDescriptor sqlNpcDescriptor = new SqlLocationResourceDescriptor(typeof(Models.Npc), "npcs", ".{id}");

                    o.DescriptorService = new ResourceDescriptorService();
                    o.DescriptorService.Register<Models.Quest>(sqlQuestDescriptor);
                    o.DescriptorService.Register<Models.Dialogue>(sqlDialogueDescriptor);
                    o.DescriptorService.Register<Models.Npc>(sqlNpcDescriptor);
                });
            }, null, null);
            return services.BuildServiceProvider();
        }
    }
}
