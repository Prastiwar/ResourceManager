﻿using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests
{
    public class CreateResourceTests : IntegrationTestClass
    {
        [Fact]
        public async Task CreateDialogueWithFtp()
        {
            Dialogue dialogue = Dummies.Dialogue;
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    await dataSource.AddAsync(dialogue);
                    await dataSource.SaveChangesAsync();
                    string relativePath = GetDialoguePath("Ftp", dialogue);
                    Assert.True(File.Exists(relativePath));
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
            Dialogue dialogue = Dummies.Dialogue;
            IDataSource dataSource = GetIntegratedLocalDataSource();
            await dataSource.AddAsync(dialogue);
            await dataSource.SaveChangesAsync();
            string relativePath = GetDialoguePath("Local", dialogue);
            Assert.True(File.Exists(relativePath));
        }

        [Fact]
        public async Task CreateDialogueWithSql()
        {
            Dialogue dialogue = Dummies.Dialogue;
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.AddAsync(dialogue);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task CreateQuestWithFtp()
        {
            Quest quest = Dummies.Quest;
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    await dataSource.AddAsync(quest);
                    await dataSource.SaveChangesAsync();
                    string relativePath = GetQuestPath("Ftp", quest);
                    Assert.True(File.Exists(relativePath));
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
            Quest quest = Dummies.Quest;
            IDataSource dataSource = GetIntegratedLocalDataSource();
            await dataSource.AddAsync(quest);
            await dataSource.SaveChangesAsync();
            string relativePath = GetQuestPath("Local", quest);
            Assert.True(File.Exists(relativePath));
        }

        [Fact]
        public async Task CreateQuestWithSql()
        {
            Quest quest = Dummies.Quest;
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.AddAsync(quest);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }

        [Fact]
        public async Task CreateNpcWithFtp()
        {
            Npc npc = Dummies.Npc;
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    await dataSource.AddAsync(npc);
                    await dataSource.SaveChangesAsync();
                    string relativePath = GetNpcPath("Ftp", npc);
                    Assert.True(File.Exists(relativePath));
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
            Npc npc = Dummies.Npc;
            IDataSource dataSource = GetIntegratedLocalDataSource();
            await dataSource.AddAsync(npc);
            await dataSource.SaveChangesAsync();
            string relativePath = GetNpcPath("Local", npc);
            Assert.True(File.Exists(relativePath));
        }

        [Fact]
        public async Task CreateNpcWithSql()
        {
            Npc npc = Dummies.Npc;
            IDataSource dataSource = GetIntegratedSqlDataSource();
            await dataSource.AddAsync(npc);
            await dataSource.SaveChangesAsync();
            // TODO: Test Asserts
        }
    }
}
