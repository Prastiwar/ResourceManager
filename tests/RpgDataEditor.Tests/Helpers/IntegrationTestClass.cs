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
    public abstract class IntegrationTestClass
    {
        public IntegrationTestClass() => ServiceProvider = ConfigureServices(new ServiceCollection());

        protected IServiceProvider ServiceProvider { get; }

        private readonly int repeatIOCount = 10;

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

        /// <summary> Copies files from fixtures defined with name to temp location </summary>
        protected void InitializeFixture(string fixtureName)
        {
            string path = $"./Fixtures/{fixtureName}";
            string tempPath = $"./Fixtures/{fixtureName}-temp";
            string fullPath = Path.GetFullPath(tempPath);
            if (fullPath.StartsWith("\\\\?\\"))
            {

            }
            if (Directory.Exists(tempPath))
            {
                for (int i = 0; i < repeatIOCount; i++)
                {
                    try
                    {
                        Directory.Delete(tempPath, true);
                    }
                    catch (Exception ex) when (ex is UnauthorizedAccessException || ex is IOException)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        break;
                    }
                    break;
                }
            }
            //DirectoryInfo targetPath = Directory.CreateDirectory(tempPath);
            //CopyAll(new DirectoryInfo(path), targetPath);
        }

        protected abstract IDataSource ConnectDataSource(CancellationToken token = default);

        protected abstract void BuildDataSource(IConfigurableDataSourceBuilder builder, ResourceDescriptorService descriptorService);

        protected virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
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

        protected void CreateLocalFile(string filePath, object resource)
        {
            ITextSerializer serializer = ServiceProvider.GetRequiredService<ITextSerializer>();
            string text = serializer.Serialize(resource);
            string fullFilePath = Path.GetFullPath(filePath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));
            for (int i = 0; i < repeatIOCount; i++)
            {
                try
                {
                    File.WriteAllText(fullFilePath, text);
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }
                break;
            }
        }

        protected T GetLocalResource<T>(string filePath)
        {
            ITextSerializer serializer = ServiceProvider.GetRequiredService<ITextSerializer>();
            for (int i = 0; i < repeatIOCount; i++)
            {
                try
                {
                    string text = File.ReadAllText(filePath);
                    return (T)serializer.Deserialize(text, typeof(T));
                }
                catch (IOException)
                {
                    continue;
                }
            }
            throw new InvalidProgramException();
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
