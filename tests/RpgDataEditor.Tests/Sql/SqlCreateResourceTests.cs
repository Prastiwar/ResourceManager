using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Sql
{
    public class SqlCreateResourceTests
    {
        [Fact]
        public async Task CreateDialogue()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Dialogue dialogue = Dummies.Dialogue;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.AddAsync(dialogue);
                await dataSource.SaveChangesAsync();
                Assert.Contains(dialogue, dataSource.Query<Dialogue>().ToList());
            }
        }

        [Fact]
        public async Task CreateQuest()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Quest quest = Dummies.Quest;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.AddAsync(quest);
                await dataSource.SaveChangesAsync();
                Assert.Contains(quest, dataSource.Query<Quest>().ToList());
            }
        }

        [Fact]
        public async Task CreateNpc()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Npc npc = Dummies.Npc;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.AddAsync(npc);
                await dataSource.SaveChangesAsync();
                Assert.Contains(npc, dataSource.Query<Npc>().ToList());
            }
        }
    }
}
