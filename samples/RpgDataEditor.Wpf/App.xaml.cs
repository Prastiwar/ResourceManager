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

            ResourceDescriptorService descriptorService = new ResourceDescriptorService();
            IResourceDescriptor questDescriptor = new LocationResourceDescriptor(typeof(Quest), "/quests", "/{category}/{id}_{title}.json");
            IResourceDescriptor dialogueDescriptor = new LocationResourceDescriptor(typeof(Dialogue), "/dialogues", "/{category}/{id}_{title}.json");
            IResourceDescriptor npcDescriptor = new LocationResourceDescriptor(typeof(Npc), "/npcs", "/{id}_{name}.json");
            descriptorService.Register<Quest>(questDescriptor);
            descriptorService.Register<Dialogue>(dialogueDescriptor);
            descriptorService.Register<Npc>(npcDescriptor);

            builder.AddLocalDataSource(o => {
                o.DescriptorService = descriptorService;
                o.Serializer = serializer;
            });

            builder.AddFtpDataSource(o => {
                o.DescriptorService = descriptorService;
                o.Serializer = serializer;
            });

            builder.AddSqlDataSource(o => {
                o.DescriptorService = descriptorService;
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
