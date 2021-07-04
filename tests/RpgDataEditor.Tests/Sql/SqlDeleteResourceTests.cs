using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Sql
{

    [Collection(NonParallelCollectionDefinition.NAME)]
    public class SqlDeleteResourceTests
    {
        [Fact]
        public async Task DeleteDialogue()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Dialogue resource = Dummies.DeleteDialogue;
                IDataSource dataSource = integration.ConnectDataSource();
                bool hasResource = dataSource.Query<Dialogue>().ToList().Contains(resource);
                if (!hasResource)
                {
                    await dataSource.AddAsync(resource);
                    await dataSource.SaveChangesAsync();
                    hasResource = dataSource.Query<Dialogue>().ToList().Contains(resource);
                    Assert.True(hasResource);
                }
                await dataSource.DeleteAsync(resource);
                await dataSource.SaveChangesAsync();
                Assert.DoesNotContain(resource, dataSource.Query<Dialogue>().ToList());
            }
        }

        [Fact]
        public async Task DeleteQuest()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Quest resource = Dummies.DeleteQuest;
                IDataSource dataSource = integration.ConnectDataSource();
                bool hasResource = dataSource.Query<Quest>().ToList().Contains(resource);
                if (!hasResource)
                {
                    await dataSource.AddAsync(resource);
                    await dataSource.SaveChangesAsync();
                    hasResource = dataSource.Query<Quest>().ToList().Contains(resource);
                    Assert.True(hasResource);
                }
                await dataSource.DeleteAsync(resource);
                await dataSource.SaveChangesAsync();
                Assert.DoesNotContain(resource, dataSource.Query<Quest>().ToList());
            }
        }

        [Fact]
        public async Task DeleteNpc()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Npc resource = Dummies.DeleteNpc;
                IDataSource dataSource = integration.ConnectDataSource();
                bool hasResource = dataSource.Query<Npc>().ToList().Contains(resource);
                if (!hasResource)
                {
                    await dataSource.AddAsync(resource);
                    await dataSource.SaveChangesAsync();
                    hasResource = dataSource.Query<Npc>().ToList().Contains(resource);
                    Assert.True(hasResource);
                }
                await dataSource.DeleteAsync(resource);
                await dataSource.SaveChangesAsync();
                Assert.DoesNotContain(resource, dataSource.Query<Npc>().ToList());
            }
        }
    }
}
