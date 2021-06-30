using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Ftp
{
    [Collection(NonParallelCollectionDefinition.NAME)]
    public class FtpUpdateResourceTests
    {
        [Fact]
        public async Task UpdateDialogue()
        {
            using (FtpIntegrationTestProvider integration = new FtpIntegrationTestProvider())
            {
                Dialogue dialogue = Dummies.UpdateDialogue;
                IDataSource dataSource = integration.ConnectDataSource();
                string relativePath = integration.GetDialoguePath(dialogue);
                if (!File.Exists(relativePath))
                {
                    integration.CreateLocalFile(relativePath, dialogue);
                }
                Assert.True(File.Exists(relativePath));
                dataSource.Attach(dialogue);
                dialogue.Message = Guid.NewGuid().ToString();
                await dataSource.UpdateAsync(dialogue);
                await dataSource.SaveChangesAsync();
                Dialogue dialogueFromFile = integration.GetLocalResource<Dialogue>(relativePath);
                Assert.NotNull(dialogueFromFile);
                Assert.Equal(dialogue.Message, dialogueFromFile.Message);
            }
        }

        [Fact]
        public async Task UpdateQuest()
        {
            using (FtpIntegrationTestProvider integration = new FtpIntegrationTestProvider())
            {
                Quest quest = Dummies.UpdateQuest;
                IDataSource dataSource = integration.ConnectDataSource();
                string relativePath = integration.GetQuestPath(quest);
                if (!File.Exists(relativePath))
                {
                    integration.CreateLocalFile(relativePath, quest);
                }
                Assert.True(File.Exists(relativePath));
                dataSource.Attach(quest);
                quest.Message = Guid.NewGuid().ToString();
                await dataSource.UpdateAsync(quest);
                await dataSource.SaveChangesAsync();
                Quest fromFile = integration.GetLocalResource<Quest>(relativePath);
                Assert.NotNull(fromFile);
                Assert.Equal(quest.Message, fromFile.Message);
            }
        }

        [Fact]
        public async Task UpdateNpc()
        {
            using (FtpIntegrationTestProvider integration = new FtpIntegrationTestProvider())
            {
                Npc npc = Dummies.UpdateNpc;
                IDataSource dataSource = integration.ConnectDataSource();
                string relativePath = integration.GetNpcPath(npc);
                if (!File.Exists(relativePath))
                {
                    integration.CreateLocalFile(relativePath, npc);
                }
                Assert.True(File.Exists(relativePath));
                dataSource.Attach(npc);
                npc.TalkData = new TalkData() { TalkRange = new Random().Next(0, int.MaxValue) };
                await dataSource.UpdateAsync(npc);
                await dataSource.SaveChangesAsync();
                Npc fromFile = integration.GetLocalResource<Npc>(relativePath);
                Assert.NotNull(fromFile);
                Assert.Equal(npc.TalkData.TalkRange, fromFile.TalkData.TalkRange);
            }
        }
    }
}
