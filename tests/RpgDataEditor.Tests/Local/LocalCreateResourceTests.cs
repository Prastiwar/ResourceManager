using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Local
{
    [Collection(NonParallelCollectionDefinition.NAME)]
    public class LocalCreateResourceTests
    {
        [Fact]
        public async Task CreateDialogue()
        {
            using (LocalIntegrationTestProvider integration = new LocalIntegrationTestProvider())
            {
                Dialogue dialogue = Dummies.Dialogue;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.AddAsync(dialogue);
                await dataSource.SaveChangesAsync();
                string relativePath = integration.GetDialoguePath(dialogue);
                Assert.True(File.Exists(relativePath));
            }
        }

        [Fact]
        public async Task CreateQuest()
        {
            using (LocalIntegrationTestProvider integration = new LocalIntegrationTestProvider())
            {
                Quest quest = Dummies.Quest;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.AddAsync(quest);
                await dataSource.SaveChangesAsync();
                string relativePath = integration.GetQuestPath(quest);
                Assert.True(File.Exists(relativePath));
            }
        }

        [Fact]
        public async Task CreateNpc()
        {
            using (LocalIntegrationTestProvider integration = new LocalIntegrationTestProvider())
            {
                Npc npc = Dummies.Npc;
                IDataSource dataSource = integration.ConnectDataSource();
                await dataSource.AddAsync(npc);
                await dataSource.SaveChangesAsync();
                string relativePath = integration.GetNpcPath(npc);
                Assert.True(File.Exists(relativePath));
            }
        }
    }
}
