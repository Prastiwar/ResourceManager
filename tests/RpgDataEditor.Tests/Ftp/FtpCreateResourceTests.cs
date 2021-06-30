using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Ftp
{
    public class FtpCreateResourceTests : FtpIntegrationTestClass
    {
        [Fact]
        public async Task CreateDialogue()
        {
            Dialogue dialogue = Dummies.Dialogue;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.AddAsync(dialogue);
            await dataSource.SaveChangesAsync();
            string relativePath = GetDialoguePath("Ftp", dialogue);
            Assert.True(File.Exists(relativePath));
        }

        [Fact]
        public async Task CreateQuest()
        {
            Quest quest = Dummies.Quest;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.AddAsync(quest);
            await dataSource.SaveChangesAsync();
            string relativePath = GetQuestPath("Ftp", quest);
            Assert.True(File.Exists(relativePath));
        }

        [Fact]
        public async Task CreateNpc()
        {
            Npc npc = Dummies.Npc;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.AddAsync(npc);
            await dataSource.SaveChangesAsync();
            string relativePath = GetNpcPath("Ftp", npc);
            Assert.True(File.Exists(relativePath));
        }
    }
}
