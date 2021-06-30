using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Sql
{
    public class SqlDeleteResourceTests
    {
        [Fact]
        public async Task DeleteDialogue()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Dialogue dialogue = Dummies.DeleteDialogue;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.DeleteAsync(dialogue);
                await dataSource.SaveChangesAsync();
                // TODO: Test Asserts
            }
        }

        [Fact]
        public async Task DeleteQuest()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Quest quest = Dummies.DeleteQuest;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.DeleteAsync(quest);
                await dataSource.SaveChangesAsync();
                // TODO: Test Asserts
            }
        }

        [Fact]
        public async Task DeleteNpc()
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                Npc npc = Dummies.DeleteNpc;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.DeleteAsync(npc);
                await dataSource.SaveChangesAsync();
                // TODO: Test Asserts
            }
        }
    }
}
