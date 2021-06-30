using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Ftp
{
    [Collection(NonParallelCollectionDefinition.NAME)]
    public class FtpCreateResourceTests
    {
        [Fact]
        public async Task CreateDialogue()
        {
            using (FtpIntegrationTestProvider integration = new FtpIntegrationTestProvider())
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
            using (FtpIntegrationTestProvider integration = new FtpIntegrationTestProvider())
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
            using (FtpIntegrationTestProvider integration = new FtpIntegrationTestProvider())
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
