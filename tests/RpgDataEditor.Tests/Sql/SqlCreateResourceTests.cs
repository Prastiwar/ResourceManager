using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Sql
{

    [Collection(NonParallelCollectionDefinition.NAME)]
    public class SqlCreateResourceTests
    {
        [Fact]
        public async Task CreateDialogue()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Dialogue resource = Dummies.Dialogue;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.AddAsync(resource);
                await dataSource.SaveChangesAsync();
                Assert.Contains(resource, dataSource.Query<Dialogue>().ToList());
            }
        }

        [Fact]
        public async Task CreateQuest()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Quest resource = Dummies.Quest;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.AddAsync(resource);
                await dataSource.SaveChangesAsync();
                Assert.Contains(resource, dataSource.Query<Quest>().ToList());
            }
        }

        [Fact]
        public async Task CreateNpc()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Npc resource = Dummies.Npc;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.AddAsync(resource);
                await dataSource.SaveChangesAsync();
                Assert.Contains(resource, dataSource.Query<Npc>().ToList());
            }
        }
    }
}
