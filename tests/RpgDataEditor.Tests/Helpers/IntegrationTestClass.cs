using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ResourceManager;
using ResourceManager.Core.Serialization;
using ResourceManager.Core.Services;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.DataSource.Sql.Configuration;
using ResourceManager.DataSource.Sql.Data;
using ResourceManager.Services;
using ResourceManager.Wpf.Converters;
using RpgDataEditor.DataSource;
using RpgDataEditor.Models;
using RpgDataEditor.Serialization;
using System;
using System.IO;
using System.Threading;

namespace RpgDataEditor.Tests
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
                server.Start();
                token.Register(server.Stop);
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
            JsonConvert.DefaultSettings = CreateJsonSettings;
            services.AddSingleton<ITextSerializer, NewtonsoftSerializer>();
            services.AddDataSourceConfiguration(builder => {
                builder.RegisterResourceTypes(typeof(Quest), typeof(Dialogue), typeof(Npc));

                ResourceDescriptorService fileDescriptorService = new ResourceDescriptorService();
                IResourceDescriptor fileQuestDescriptor = new LocationResourceDescriptor(typeof(Quest), "/quests", "/{category}/{id}_{title}.json");
                IResourceDescriptor fileDialogueDescriptor = new LocationResourceDescriptor(typeof(Dialogue), "/dialogues", "/{category}/{id}_{title}.json");
                IResourceDescriptor fileNpcDescriptor = new LocationResourceDescriptor(typeof(Npc), "/npcs", "/{id}_{name}.json");
                fileDescriptorService.Register<Quest>(fileQuestDescriptor);
                fileDescriptorService.Register<Dialogue>(fileDialogueDescriptor);
                fileDescriptorService.Register<Npc>(fileNpcDescriptor);

                builder.AddLocalDataSource(o => {
                    o.DescriptorService = fileDescriptorService;
                    o.Serializer = serializer;
                });

                builder.AddFtpDataSource(o => {
                    o.DescriptorService = fileDescriptorService;
                    o.Serializer = serializer;
                });

                builder.AddSqlDataSource(o => {
                    IResourceDescriptor sqlQuestDescriptor = new SqlLocationResourceDescriptor(typeof(Quest), "quests", ".{id}");
                    IResourceDescriptor sqlDialogueDescriptor = new SqlLocationResourceDescriptor(typeof(Dialogue), "dialogues", ".{id}");
                    IResourceDescriptor sqlNpcDescriptor = new SqlLocationResourceDescriptor(typeof(Npc), "npcs", ".{id}");

                    o.DescriptorService = new ResourceDescriptorService();
                    o.DescriptorService.Register<Quest>(sqlQuestDescriptor);
                    o.DescriptorService.Register<Dialogue>(sqlDialogueDescriptor);
                    o.DescriptorService.Register<Npc>(sqlNpcDescriptor);
                    o.CreateDatabaseContext = CreateSqlDbContext;
                });
            }, null, null);
            return services.BuildServiceProvider();
        }

        protected virtual DbContext CreateSqlDbContext(string connectionString, IConfiguration configuration, SqlDataSourceOptions options)
            => new DefaultDbContext(connectionString, configuration);

        protected void CreateLocalFile(string filePath, object resource)
        {
            ITextSerializer serializer = ServiceProvider.GetRequiredService<ITextSerializer>();
            string text = serializer.Serialize(resource);
            string fullFilePath = Path.GetFullPath(filePath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));
            File.WriteAllText(fullFilePath, text);
        }

        protected T GetLocalResource<T>(string filePath)
        {
            ITextSerializer serializer = ServiceProvider.GetRequiredService<ITextSerializer>();
            string text = File.ReadAllText(filePath);
            return (T)serializer.Deserialize(text, typeof(T));
        }

        protected virtual JsonSerializerSettings CreateJsonSettings()
        {
            PrettyOrderPropertyResolver propResolver = new PrettyOrderPropertyResolver();
            propResolver.SetAllLetterCase(Lettercase.CamelCase);
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = propResolver,
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new NumberCastsConverter());
            settings.Converters.Add(new ConfigurationSectionJsonConverter());

            settings.Converters.Add(new PlayerRequirementJsonConverter());

            settings.Converters.Add(new NpcJsonConverter());
            settings.Converters.Add(new NpcJobJsonConverter());
            settings.Converters.Add(new TradeItemJsonConverter());
            settings.Converters.Add(new AttributeDataModelJsonConverter());

            settings.Converters.Add(new PositionJsonConverter());

            settings.Converters.Add(new QuestTaskJsonConverter());
            settings.Converters.Add(new QuestJsonConverter());

            settings.Converters.Add(new DialogueJsonConverter());
            settings.Converters.Add(new DialogueOptionJsonConverter());
            settings.Converters.Add(new TalkDataJsonConverter());
            settings.Converters.Add(new TalkLineJsonConverter());
            return settings;
        }

        protected string GetDialoguePath(string dataSourceName, Dialogue dialogue)
        {
            string path = $"./Fixtures/{dataSourceName}-temp/dialogues";
            if (dialogue != null)
            {
                path += $"/{dialogue.Category}/{dialogue.Id}_{dialogue.Title}.json";
            }
            return path;
        }

        protected string GetQuestPath(string dataSourceName, Quest quest)
        {
            string path = $"./Fixtures/{dataSourceName}-temp/quests";
            if (quest != null)
            {
                path += $"/{quest.Category}/{quest.Id}_{quest.Title}.json";
            }
            return path;
        }

        protected string GetNpcPath(string dataSourceName, Npc npc)
        {
            string path = $"./Fixtures/{dataSourceName}-temp/npcs";
            if (npc != null)
            {
                path += $"/{npc.Id}_{npc.Name}.json";
            }
            return path;
        }
    }
}
