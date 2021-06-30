using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ResourceManager;
using ResourceManager.Core.Serialization;
using ResourceManager.Core.Services;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Services;
using RpgDataEditor.Models;
using RpgDataEditor.Serialization;
using System;
using System.IO;
using System.Threading;

namespace RpgDataEditor.Tests
{
    public abstract class IntegrationTestProvider : IDisposable
    {
        public IntegrationTestProvider(string key)
        {
            TempKey = key;
            ServiceProvider = ConfigureServices(new ServiceCollection());
            InitializeFixture();
        }

        protected IServiceProvider ServiceProvider { get; }

        public string TempKey { get; set; }

        protected abstract string DataSourceName { get; }

        protected virtual string FixtureFolderName => "Fixtures";

        protected virtual string RootPath => $"./{FixtureFolderName}/{DataSourceName}-{TempKey}";

        /// <summary> Clears fixtures temp location and creates new using TempKey </summary>
        protected void InitializeFixture()
        {
            DisposedCheck();
            if (string.IsNullOrEmpty(TempKey))
            {
                TempKey = Guid.NewGuid().ToString();
            }
            string tempPath = RootPath;
            if (Directory.Exists(tempPath))
            {
                try
                {
                    RetryHelper.TryOperation(() => Directory.Delete(tempPath, true), RetryHelper.UnauthorizedIOExceptions);
                }
                catch (DirectoryNotFoundException)
                {
                    // Directory was already deleted
                }
            }
            RetryHelper.TryOperation(() => Directory.CreateDirectory(tempPath), RetryHelper.UnauthorizedIOExceptions);
        }

        public abstract IDataSource ConnectDataSource(CancellationToken token = default);

        protected abstract void BuildDataSource(IConfigurableDataSourceBuilder builder, ResourceDescriptorService descriptorService);

        protected virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            DisposedCheck();
            services.AddConfiguration(builder => builder.AddInMemoryCollection());
            NewtonsoftSerializer serializer = new NewtonsoftSerializer();
            JsonConvert.DefaultSettings = CreateJsonSettings;
            services.AddSingleton<ITextSerializer, NewtonsoftSerializer>();
            services.AddDataSourceConfiguration(builder => {
                builder.RegisterResourceTypes(typeof(Quest), typeof(Dialogue), typeof(Npc));

                ResourceDescriptorService descriptorService = new ResourceDescriptorService();
                IResourceDescriptor questDescriptor = new LocationResourceDescriptor(typeof(Quest), "/quests", "/{category}/{id}_{title}.json");
                IResourceDescriptor dialogueDescriptor = new LocationResourceDescriptor(typeof(Dialogue), "/dialogues", "/{category}/{id}_{title}.json");
                IResourceDescriptor npcDescriptor = new LocationResourceDescriptor(typeof(Npc), "/npcs", "/{id}_{name}.json");
                descriptorService.Register<Quest>(questDescriptor);
                descriptorService.Register<Dialogue>(dialogueDescriptor);
                descriptorService.Register<Npc>(npcDescriptor);

                BuildDataSource(builder, descriptorService);
            }, null, null);
            return services.BuildServiceProvider();
        }

        public void CreateLocalFile(string filePath, object resource)
        {
            DisposedCheck();
            ITextSerializer serializer = ServiceProvider.GetRequiredService<ITextSerializer>();
            string text = serializer.Serialize(resource);
            string fullFilePath = Path.GetFullPath(filePath);
            RetryHelper.TryOperation(() => Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath)), RetryHelper.DirectoryNotFoundExceptions);
            RetryHelper.TryOperation(() => File.WriteAllText(fullFilePath, text), RetryHelper.DirectoryNotFoundExceptions);
        }

        public T GetLocalResource<T>(string filePath)
        {
            DisposedCheck();
            ITextSerializer serializer = ServiceProvider.GetRequiredService<ITextSerializer>();
            string text = RetryHelper.TryOperation(() => File.ReadAllText(filePath), RetryHelper.IOExceptionsFilter);
            return (T)serializer.Deserialize(text, typeof(T));
        }

        protected virtual JsonSerializerSettings CreateJsonSettings()
        {
            DisposedCheck();
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

        public string GetDialoguePath(Dialogue dialogue)
        {
            DisposedCheck();
            string path = $"{RootPath}/dialogues";
            if (dialogue != null)
            {
                path += $"/{dialogue.Category}/{dialogue.Id}_{dialogue.Title}.json";
            }
            return path;
        }

        public string GetQuestPath(Quest quest)
        {
            DisposedCheck();
            string path = $"{RootPath}/quests";
            if (quest != null)
            {
                path += $"/{quest.Category}/{quest.Id}_{quest.Title}.json";
            }
            return path;
        }

        public string GetNpcPath(Npc npc)
        {
            DisposedCheck();
            string path = $"{RootPath}/npcs";
            if (npc != null)
            {
                path += $"/{npc.Id}_{npc.Name}.json";
            }
            return path;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    RetryHelper.TryOperation(() => Directory.Delete(RootPath, true), RetryHelper.UnauthorizedIOExceptions);
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void DisposedCheck()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
