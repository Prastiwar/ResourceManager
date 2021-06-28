using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests
{
    public class UpdateResourceTests : IntegrationTestClass
    {
        [Fact]
        public async Task UpdateDialogueWithFtp()
        {
            Dialogue dialogue = Dummies.UpdateDialogue;
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    string relativePath = GetDialoguePath("Ftp", dialogue);
                    if (!File.Exists(relativePath))
                    {
                        CreateLocalFile(relativePath, dialogue);
                    }
                    Assert.True(File.Exists(relativePath));
                    dialogue.Message = Guid.NewGuid().ToString();
                    await dataSource.UpdateAsync(dialogue);
                    await dataSource.SaveChangesAsync();
                    Dialogue dialogueFromFile = GetLocalResource<Dialogue>(relativePath);
                    Assert.NotNull(dialogueFromFile);
                    Assert.Equal(dialogue.Message, dialogueFromFile.Message);
                }
                catch (Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        [Fact]
        public async Task UpdateDialogueWithLocal()
        {
            Dialogue dialogue = Dummies.UpdateDialogue;
            IDataSource dataSource = GetIntegratedLocalDataSource();
            string relativePath = GetDialoguePath("Local", dialogue);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, dialogue);
            }
            Assert.True(File.Exists(relativePath));
            dialogue.Message = Guid.NewGuid().ToString();
            await dataSource.UpdateAsync(dialogue);
            await dataSource.SaveChangesAsync();
            Dialogue fromFile = GetLocalResource<Dialogue>(relativePath);
            Assert.NotNull(fromFile);
            Assert.Equal(dialogue.Message, fromFile.Message);
        }

        [Fact]
        public async Task UpdateDialogueWithSql()
        {
            Dialogue dialogue = Dummies.UpdateDialogue;
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.UpdateAsync(dialogue);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task UpdateQuestWithFtp()
        {
            Quest quest = Dummies.UpdateQuest;
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    string relativePath = GetQuestPath("Ftp", quest);
                    if (!File.Exists(relativePath))
                    {
                        CreateLocalFile(relativePath, quest);
                    }
                    Assert.True(File.Exists(relativePath));
                    quest.Message = Guid.NewGuid().ToString();
                    await dataSource.UpdateAsync(quest);
                    await dataSource.SaveChangesAsync();
                    Quest fromFile = GetLocalResource<Quest>(relativePath);
                    Assert.NotNull(fromFile);
                    Assert.Equal(quest.Message, fromFile.Message);
                }
                catch (Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        [Fact]
        public async Task UpdateQuestWithLocal()
        {
            Quest quest = Dummies.UpdateQuest;
            IDataSource dataSource = GetIntegratedLocalDataSource();
            string relativePath = GetQuestPath("Local", quest);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, quest);
            }
            Assert.True(File.Exists(relativePath));
            quest.Message = Guid.NewGuid().ToString();
            await dataSource.UpdateAsync(quest);
            await dataSource.SaveChangesAsync();
            Quest fromFile = GetLocalResource<Quest>(relativePath);
            Assert.NotNull(fromFile);
            Assert.Equal(quest.Message, fromFile.Message);
        }

        [Fact]
        public async Task UpdateQuestWithSql()
        {
            Quest quest = Dummies.UpdateQuest;
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.UpdateAsync(quest);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task UpdateNpcWithFtp()
        {
            Npc npc = Dummies.UpdateNpc;
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    string relativePath = GetNpcPath("Ftp", npc);
                    if (!File.Exists(relativePath))
                    {
                        CreateLocalFile(relativePath, npc);
                    }
                    Assert.True(File.Exists(relativePath));
                    npc.TalkData = new TalkData() { TalkRange = new Random().Next(0, int.MaxValue) };
                    await dataSource.UpdateAsync(npc);
                    await dataSource.SaveChangesAsync();
                    Npc fromFile = GetLocalResource<Npc>(relativePath);
                    Assert.NotNull(fromFile);
                    Assert.Equal(npc.TalkData.TalkRange, fromFile.TalkData.TalkRange);
                }
                catch (Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        [Fact]
        public async Task UpdateNpcWithLocal()
        {
            Npc npc = Dummies.UpdateNpc;
            IDataSource dataSource = GetIntegratedLocalDataSource();
            string relativePath = GetNpcPath("Local", npc);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, npc);
            }
            Assert.True(File.Exists(relativePath));
            npc.TalkData = new TalkData() { TalkRange = new Random().Next(0, int.MaxValue) };
            await dataSource.UpdateAsync(npc);
            await dataSource.SaveChangesAsync();
            Npc fromFile = GetLocalResource<Npc>(relativePath);
            Assert.NotNull(fromFile);
            Assert.Equal(npc.TalkData.TalkRange, fromFile.TalkData.TalkRange);
        }

        [Fact]
        public async Task UpdateNpcWithSql()
        {
            Npc npc = Dummies.UpdateNpc;
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.UpdateAsync(npc);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }
    }
}
