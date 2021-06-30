using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Sql
{
    public class SqlCreateResourceTests : SqlIntegrationTestClass
    {
        [Fact]
        public async Task CreateDialogue()
        {
            Dialogue dialogue = Dummies.Dialogue;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.AddAsync(dialogue);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task CreateQuest()
        {
            Quest quest = Dummies.Quest;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.AddAsync(quest);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task CreateNpc()
        {
            Npc npc = Dummies.Npc;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.AddAsync(npc);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }
    }
}
