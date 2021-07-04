using ResourceManager;
using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Sql
{

    [Collection(NonParallelCollectionDefinition.NAME)]
    public class SqlUpdateResourceTests
    {
        [Fact]
        public async Task UpdateDialogue()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Dialogue resource = Dummies.UpdateDialogue;
                IDataSource dataSource = integration.ConnectDataSource();
                bool hasResource = dataSource.Query<Dialogue>().ToList().Contains(resource);
                if (!hasResource)
                {
                    await dataSource.AddAsync(resource);
                    await dataSource.SaveChangesAsync();
                    hasResource = dataSource.Query<Dialogue>().ToList().Contains(resource);
                    Assert.True(hasResource);
                }
                dataSource.Attach(resource);
                resource.Message = Guid.NewGuid().ToString();
                await dataSource.UpdateAsync(resource);
                await dataSource.SaveChangesAsync();
                Dialogue updated = dataSource.Query<Dialogue>().ToList().First(d => IdentifiableComparer.Default.Compare(d, resource) == 0);
                Assert.Equal(resource.Message, updated.Message);
            }
        }

        [Fact]
        public async Task UpdateQuest()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Quest resource = Dummies.UpdateQuest;
                IDataSource dataSource = integration.ConnectDataSource();
                bool hasResource = dataSource.Query<Quest>().ToList().Contains(resource);
                if (!hasResource)
                {
                    await dataSource.AddAsync(resource);
                    await dataSource.SaveChangesAsync();
                    hasResource = dataSource.Query<Quest>().ToList().Contains(resource);
                    Assert.True(hasResource);
                }
                dataSource.Attach(resource);
                resource.Message = Guid.NewGuid().ToString();
                await dataSource.UpdateAsync(resource);
                await dataSource.SaveChangesAsync();
                Quest updated = dataSource.Query<Quest>().ToList().First(d => IdentifiableComparer.Default.Compare(d, resource) == 0);
                Assert.Equal(resource.Message, updated.Message);
            }
        }

        [Fact]
        public async Task UpdateNpc()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Npc resource = Dummies.UpdateNpc;
                IDataSource dataSource = integration.ConnectDataSource();
                bool hasResource = dataSource.Query<Npc>().ToList().Contains(resource);
                if (!hasResource)
                {
                    await dataSource.AddAsync(resource);
                    await dataSource.SaveChangesAsync();
                    hasResource = dataSource.Query<Npc>().ToList().Contains(resource);
                    Assert.True(hasResource);
                }
                dataSource.Attach(resource);
                resource.TalkData.TalkRange = new Random().Next(0, int.MaxValue);
                await dataSource.UpdateAsync(resource);
                await dataSource.SaveChangesAsync();
                Npc updated = dataSource.Query<Npc>().ToList().First(d => IdentifiableComparer.Default.Compare(d, resource) == 0);
                Assert.Equal(resource.TalkData.TalkRange, updated.TalkData.TalkRange);
            }
        }
    }
}
