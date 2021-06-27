using ResourceManager.DataSource;
using RPGDataEditor.Sample.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RPGDataEditor.Sample.Tests
{
    public class CreateResourceTests : IntegrationTestClass
    {
        private readonly Dialogue dialogue = new Dialogue() {
            Id = int.MaxValue / 2,
            StartQuestId = 1,
            Title = "TestTitle",
            Category = "TestCategory",
            Message = "TestMessage",
            Requirements = new List<Requirement>(),
            Options = new List<DialogueOption>()
        };

        private readonly Quest quest = new Quest() {
            Id = int.MaxValue / 2,
            Title = "TestTitle",
            Category = "TestCategory",
            Message = "TestMessage",
            CompletionTask = new ReachQuestTask(),
            Requirements = new List<Requirement>(),
        };

        private readonly Npc npc = new Npc() {
            Id = int.MaxValue / 2,
            Name = "TestName",
            Job = new TraderNpcJob() {
                Items = new List<TradeItem>() {
                    new TradeItem() {
                        ItemId = "id",
                        Buy = 10,
                    }
                }
            },
            Position = new Position(),
            TalkData = new TalkData(),
            Attributes = new List<AttributeData>(),
        };

        [Fact]
        public async Task CreateDialogueWithFtp()
        {
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    await dataSource.AddAsync(dialogue);
                    await dataSource.SaveChangesAsync();
                    string relativePath = $"./Fixtures/Ftp-temp/dialogues/{dialogue.Category}/{dialogue.Id}_{dialogue.Title}.json";
                    FileInfo newFile = new FileInfo(relativePath);
                    Assert.True(newFile.Exists);
                }
                catch (System.Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        [Fact]
        public async Task CreateDialogueWithLocal()
        {
            IDataSource dataSource = GetIntegratedLocalDataSource();
            await dataSource.AddAsync(dialogue);
            await dataSource.SaveChangesAsync();
            string relativePath = $"./Fixtures/Local-temp/dialogues/{dialogue.Category}/{dialogue.Id}_{dialogue.Title}.json";
            FileInfo newFile = new FileInfo(relativePath);
            Assert.True(newFile.Exists);
        }

        [Fact]
        public async Task CreateDialogueWithSql()
        {
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.AddAsync(dialogue);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task CreateQuestWithFtp()
        {
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    await dataSource.AddAsync(quest);
                    await dataSource.SaveChangesAsync();
                    string relativePath = $"./Fixtures/Ftp-temp/quests/{quest.Category}/{quest.Id}_{quest.Title}.json";
                    FileInfo newFile = new FileInfo(relativePath);
                    Assert.True(newFile.Exists);
                }
                catch (System.Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        [Fact]
        public async Task CreateQuestWithLocal()
        {
            IDataSource dataSource = GetIntegratedLocalDataSource();
            await dataSource.AddAsync(quest);
            await dataSource.SaveChangesAsync();
            string relativePath = $"./Fixtures/Local-temp/quests/{quest.Category}/{quest.Id}_{quest.Title}.json";
            FileInfo newFile = new FileInfo(relativePath);
            Assert.True(newFile.Exists);
        }

        [Fact]
        public async Task CreateQuestWithSql()
        {
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.AddAsync(quest);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task CreateNpcWithFtp()
        {
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    await dataSource.AddAsync(npc);
                    await dataSource.SaveChangesAsync();
                    string relativePath = $"./Fixtures/Ftp-temp/npcs/{npc.Id}_{npc.Name}.json";
                    FileInfo newFile = new FileInfo(relativePath);
                    Assert.True(newFile.Exists);
                }
                catch (System.Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        [Fact]
        public async Task CreateNpcWithLocal()
        {
            IDataSource dataSource = GetIntegratedLocalDataSource();
            await dataSource.AddAsync(npc);
            await dataSource.SaveChangesAsync();
            string relativePath = $"./Fixtures/Local-temp/npcs/{npc.Id}_{npc.Name}.json";
            FileInfo newFile = new FileInfo(relativePath);
            Assert.True(newFile.Exists);
        }

        [Fact]
        public async Task CreateNpcWithSql()
        {
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.AddAsync(npc);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }
    }
}
