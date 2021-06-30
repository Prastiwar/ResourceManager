﻿using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Ftp
{
    public class FtpUpdateResourceTests : FtpIntegrationTestClass
    {
        [Fact]
        public async Task UpdateDialogue()
        {
            Dialogue dialogue = Dummies.UpdateDialogue;
            IDataSource dataSource = ConnectDataSource();
            string relativePath = GetDialoguePath("Ftp", dialogue);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, dialogue);
            }
            Assert.True(File.Exists(relativePath));
            dataSource.Attach(dialogue);
            dialogue.Message = Guid.NewGuid().ToString();
            await dataSource.UpdateAsync(dialogue);
            await dataSource.SaveChangesAsync();
            Dialogue dialogueFromFile = GetLocalResource<Dialogue>(relativePath);
            Assert.NotNull(dialogueFromFile);
            Assert.Equal(dialogue.Message, dialogueFromFile.Message);
        }

        [Fact]
        public async Task UpdateQuest()
        {
            Quest quest = Dummies.UpdateQuest;
            IDataSource dataSource = ConnectDataSource();
            string relativePath = GetQuestPath("Ftp", quest);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, quest);
            }
            Assert.True(File.Exists(relativePath));
            dataSource.Attach(quest);
            quest.Message = Guid.NewGuid().ToString();
            await dataSource.UpdateAsync(quest);
            await dataSource.SaveChangesAsync();
            Quest fromFile = GetLocalResource<Quest>(relativePath);
            Assert.NotNull(fromFile);
            Assert.Equal(quest.Message, fromFile.Message);
        }

        [Fact]
        public async Task UpdateNpc()
        {
            Npc npc = Dummies.UpdateNpc;
            IDataSource dataSource = ConnectDataSource();
            string relativePath = GetNpcPath("Ftp", npc);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, npc);
            }
            Assert.True(File.Exists(relativePath));
            dataSource.Attach(npc);
            npc.TalkData = new TalkData() { TalkRange = new Random().Next(0, int.MaxValue) };
            await dataSource.UpdateAsync(npc);
            await dataSource.SaveChangesAsync();
            Npc fromFile = GetLocalResource<Npc>(relativePath);
            Assert.NotNull(fromFile);
            Assert.Equal(npc.TalkData.TalkRange, fromFile.TalkData.TalkRange);
        }
    }
}
