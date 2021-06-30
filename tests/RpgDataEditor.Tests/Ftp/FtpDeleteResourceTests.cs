using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Ftp
{
    public class FtpDeleteResourceTests : FtpIntegrationTestClass
    {
        [Fact]
        public async Task DeleteDialogue()
        {
            Dialogue dialogue = Dummies.DeleteDialogue;
            IDataSource dataSource = ConnectDataSource();
            string relativePath = GetDialoguePath("Ftp", dialogue);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, dialogue);
            }
            Assert.True(File.Exists(relativePath));
            await dataSource.DeleteAsync(dialogue);
            await dataSource.SaveChangesAsync();
            Assert.False(File.Exists(relativePath));
        }

        [Fact]
        public async Task DeleteQuest()
        {
            Quest quest = Dummies.DeleteQuest;
            IDataSource dataSource = ConnectDataSource();
            string relativePath = GetQuestPath("Ftp", quest);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, quest);
            }
            Assert.True(File.Exists(relativePath));
            await dataSource.DeleteAsync(quest);
            await dataSource.SaveChangesAsync();
            Assert.False(File.Exists(relativePath));
        }

        [Fact]
        public async Task DeleteNpc()
        {
            Npc npc = Dummies.DeleteNpc;
            IDataSource dataSource = ConnectDataSource();
            string relativePath = GetNpcPath("Ftp", npc);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, npc);
            }
            Assert.True(File.Exists(relativePath));
            await dataSource.DeleteAsync(npc);
            await dataSource.SaveChangesAsync();
            Assert.False(File.Exists(relativePath));
        }
    }
}
