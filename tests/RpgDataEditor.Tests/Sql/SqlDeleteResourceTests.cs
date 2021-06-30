using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Sql
{
    public class SqlDeleteResourceTests : SqlIntegrationTestClass
    {
        [Fact]
        public async Task DeleteDialogue()
        {
            Dialogue dialogue = Dummies.DeleteDialogue;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.DeleteAsync(dialogue);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task DeleteQuest()
        {
            Quest quest = Dummies.DeleteQuest;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.DeleteAsync(quest);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task DeleteNpc()
        {
            Npc npc = Dummies.DeleteNpc;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.DeleteAsync(npc);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }
    }
}
