using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Sql
{
    public class SqlUpdateResourceTests : SqlIntegrationTestClass
    {
        [Fact]
        public async Task UpdateDialogue()
        {
            Dialogue dialogue = Dummies.UpdateDialogue;
            IDataSource dataSource = ConnectDataSource();
            dataSource.Attach(dialogue);
            await dataSource.UpdateAsync(dialogue);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task UpdateQuest()
        {
            Quest quest = Dummies.UpdateQuest;
            IDataSource dataSource = ConnectDataSource();
            dataSource.Attach(quest);
            await dataSource.UpdateAsync(quest);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task UpdateNpc()
        {
            Npc npc = Dummies.UpdateNpc;
            IDataSource dataSource = ConnectDataSource();
            dataSource.Attach(npc);
            await dataSource.UpdateAsync(npc);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }
    }
}
