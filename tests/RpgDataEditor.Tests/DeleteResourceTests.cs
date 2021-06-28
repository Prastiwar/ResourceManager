using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests
{
    public class DeleteResourceTests : IntegrationTestClass
    {
        [Fact]
        public async Task DeleteDialogueWithFtp()
        {
            Dialogue dialogue = Dummies.DeleteDialogue;
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
                    await dataSource.DeleteAsync(dialogue);
                    await dataSource.SaveChangesAsync();
                    Assert.False(File.Exists(relativePath));
                }
                catch (System.Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        [Fact]
        public async Task DeleteDialogueWithLocal()
        {
            Dialogue dialogue = Dummies.DeleteDialogue;
            IDataSource dataSource = GetIntegratedLocalDataSource();
            string relativePath = GetDialoguePath("Local", dialogue);
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
        public async Task DeleteDialogueWithSql()
        {
            Dialogue dialogue = Dummies.DeleteDialogue;
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.DeleteAsync(dialogue);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task DeleteQuestWithFtp()
        {
            Quest quest = Dummies.DeleteQuest;
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
                    await dataSource.DeleteAsync(quest);
                    await dataSource.SaveChangesAsync();
                    Assert.False(File.Exists(relativePath));
                }
                catch (System.Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        [Fact]
        public async Task DeleteQuestWithLocal()
        {
            Quest quest = Dummies.DeleteQuest;
            IDataSource dataSource = GetIntegratedLocalDataSource();
            string relativePath = GetQuestPath("Local", quest);
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
        public async Task DeleteQuestWithSql()
        {
            Quest quest = Dummies.DeleteQuest;
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.DeleteAsync(quest);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task DeleteNpcWithFtp()
        {
            Npc npc = Dummies.DeleteNpc;
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
                    await dataSource.DeleteAsync(npc);
                    await dataSource.SaveChangesAsync();
                    Assert.False(File.Exists(relativePath));
                }
                catch (System.Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        [Fact]
        public async Task DeleteNpcWithLocal()
        {
            Npc npc = Dummies.DeleteNpc;
            IDataSource dataSource = GetIntegratedLocalDataSource();
            string relativePath = GetNpcPath("Local", npc);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, npc);
            }
            Assert.True(File.Exists(relativePath));
            await dataSource.DeleteAsync(npc);
            await dataSource.SaveChangesAsync();
            Assert.False(File.Exists(relativePath));
        }

        [Fact]
        public async Task DeleteNpcWithSql()
        {
            Npc npc = Dummies.DeleteNpc;
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.DeleteAsync(npc);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }
    }
}
