using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Sql
{
    public class SqlUpdateResourceTests
    {
        [Fact]
        public async Task UpdateDialogue()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Dialogue dialogue = Dummies.UpdateDialogue;
                IDataSource dataSource = integration.ConnectDataSource();
                dataSource.Attach(dialogue);
                await dataSource.UpdateAsync(dialogue);
                await dataSource.SaveChangesAsync();
                // TODO: Test Asserts
            }
        }

        [Fact]
        public async Task UpdateQuest()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Quest quest = Dummies.UpdateQuest;
                IDataSource dataSource = integration.ConnectDataSource();
                dataSource.Attach(quest);
                await dataSource.UpdateAsync(quest);
                await dataSource.SaveChangesAsync();
                // TODO: Test Asserts
            }
        }

        [Fact]
        public async Task UpdateNpc()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Npc npc = Dummies.UpdateNpc;
                IDataSource dataSource = integration.ConnectDataSource();
                dataSource.Attach(npc);
                await dataSource.UpdateAsync(npc);
                await dataSource.SaveChangesAsync();
                // TODO: Test Asserts
            }
        }
    }
}
