using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Local
{
    [Collection(NonParallelCollectionDefinition.NAME)]
    public class LocalDeleteResourceTests
    {
        [Fact]
        public async Task DeleteDialogue()
        {
            using (LocalIntegrationTestProvider integration = new LocalIntegrationTestProvider())
            {
                Dialogue dialogue = Dummies.DeleteDialogue;
                IDataSource dataSource = integration.ConnectDataSource();
                string relativePath = integration.GetDialoguePath(dialogue);
                if (!File.Exists(relativePath))
                {
                    integration.CreateLocalFile(relativePath, dialogue);
                }
                Assert.True(File.Exists(relativePath));
                await dataSource.DeleteAsync(dialogue);
                await dataSource.SaveChangesAsync();
                Assert.False(File.Exists(relativePath));
            }
        }

        [Fact]
        public async Task DeleteQuest()
        {
            using (LocalIntegrationTestProvider integration = new LocalIntegrationTestProvider())
            {
                Quest quest = Dummies.DeleteQuest;
                IDataSource dataSource = integration.ConnectDataSource();
                string relativePath = integration.GetQuestPath(quest);
                if (!File.Exists(relativePath))
                {
                    integration.CreateLocalFile(relativePath, quest);
                }
                Assert.True(File.Exists(relativePath));
                await dataSource.DeleteAsync(quest);
                await dataSource.SaveChangesAsync();
                Assert.False(File.Exists(relativePath));
            }
        }

        [Fact]
        public async Task DeleteNpc()
        {
            using (LocalIntegrationTestProvider integration = new LocalIntegrationTestProvider())
            {
                Npc npc = Dummies.DeleteNpc;
                IDataSource dataSource = integration.ConnectDataSource();
                string relativePath = integration.GetNpcPath(npc);
                if (!File.Exists(relativePath))
                {
                    integration.CreateLocalFile(relativePath, npc);
                }
                Assert.True(File.Exists(relativePath));
                await dataSource.DeleteAsync(npc);
                await dataSource.SaveChangesAsync();
                Assert.False(File.Exists(relativePath));
            }
        }
    }
}
