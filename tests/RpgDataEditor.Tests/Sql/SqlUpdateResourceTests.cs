using ResourceManager;
using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System;
using System.Linq;
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
                dialogue.Message = Guid.NewGuid().ToString();
                await dataSource.UpdateAsync(dialogue);
                await dataSource.SaveChangesAsync();
                Dialogue updated = dataSource.Query<Dialogue>().ToList().First(d => IdentifiableComparer.Default.Compare(d, dialogue) == 0);
                Assert.Equal(dialogue.Message, updated.Message);
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
                quest.Message = Guid.NewGuid().ToString();
                await dataSource.UpdateAsync(quest);
                await dataSource.SaveChangesAsync();
                Quest updated = dataSource.Query<Quest>().ToList().First(d => IdentifiableComparer.Default.Compare(d, quest) == 0);
                Assert.Equal(quest.Message, updated.Message);
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
                npc.TalkData.TalkRange = new Random().Next(0, int.MaxValue);
                await dataSource.UpdateAsync(npc);
                await dataSource.SaveChangesAsync();
                Npc updated = dataSource.Query<Npc>().ToList().First(d => IdentifiableComparer.Default.Compare(d, npc) == 0);
                Assert.Equal(npc.TalkData.TalkRange, updated.TalkData.TalkRange);
            }
        }
    }
}
