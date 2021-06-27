using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Modularity;
using ResourceManager;
using ResourceManager.Core.Services;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.DataSource.Sql.Configuration;
using ResourceManager.DataSource.Sql.Data;
using ResourceManager.Services;
using ResourceManager.Wpf;
using ResourceManager.Wpf.Providers;
using RpgDataEditor.DataSource;
using RpgDataEditor.Models;
using RpgDataEditor.Serialization;
using RpgDataEditor.Wpf.Providers;
using System;

namespace RpgDataEditor.Wpf
{
    public partial class App : ResourceManagerApplication
    {
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog.AddModule<TabModule>();

        protected override void Configure(IContainerRegistry containerRegistry)
        {
            base.Configure(containerRegistry);
            containerRegistry.RegisterInstance<IAutoTemplateProvider>(new AutoTemplateProvider(Container));
        }

        protected override void ConfigureDataSources(IConfigurableDataSourceBuilder builder)
        {
            ITextSerializer serializer = new NewtonsoftSerializer();
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
        }

        protected virtual DbContext CreateSqlDbContext(string connectionString, IConfiguration configuration, SqlDataSourceOptions options)
            => new DefaultDbContext(connectionString, configuration);

        protected override JsonSerializerSettings CreateJsonSettings()
        {
            JsonSerializerSettings settings = base.CreateJsonSettings();
            settings.Converters.Add(new PlayerRequirementJsonConverter());

            settings.Converters.Add(new NpcJobJsonConverter());
            settings.Converters.Add(new NpcJsonConverter());
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

        protected override void OnConfigured(IServiceProvider provider)
        {
            base.OnConfigured(provider);
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
            IConfigurationSection dataSourceSection = configuration.GetDataSourceSection();
            if (!dataSourceSection.GetSection("EngineName").Exists())
            {
                dataSourceSection["EngineName"] = "MSSQL";
            }
        }
    }
}
