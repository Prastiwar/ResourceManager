using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Local
{
    public class LocalCreateResourceTests : LocalIntegrationTestClass
    {
        [Fact]
        public async Task CreateDialogue()
        {
            Dialogue dialogue = Dummies.Dialogue;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.AddAsync(dialogue);
            await dataSource.SaveChangesAsync();
            string relativePath = GetDialoguePath("Local", dialogue);
            Assert.True(File.Exists(relativePath));
        }

        [Fact]
        public async Task CreateQuest()
        {
            Quest quest = Dummies.Quest;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.AddAsync(quest);
            await dataSource.SaveChangesAsync();
            string relativePath = GetQuestPath("Local", quest);
            Assert.True(File.Exists(relativePath));
        }

        [Fact]
        public async Task CreateNpc()
        {
            Npc npc = Dummies.Npc;
            IDataSource dataSource = ConnectDataSource();
            await dataSource.AddAsync(npc);
            await dataSource.SaveChangesAsync();
            string relativePath = GetNpcPath("Local", npc);
            Assert.True(File.Exists(relativePath));
        }
    }
}
