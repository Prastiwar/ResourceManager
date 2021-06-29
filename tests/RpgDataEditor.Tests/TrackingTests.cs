using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests
{
    public class TrackingTests : IntegrationTestClass
    {
        [Fact]
        public async Task ThrowOnUpdateNoTracking()
        {
            IDataSource dataSource = GetIntegratedLocalDataSource();
            Dialogue dialogue = Dummies.Dialogue;
            string relativePath = GetDialoguePath("Local", dialogue);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, dialogue);
            }
            Assert.True(File.Exists(relativePath));

            await Assert.ThrowsAsync<InvalidOperationException>(() => dataSource.UpdateAsync(dialogue));
        }

        [Fact]
        public async Task DontThrowOnUpdateTracking()
        {
            IDataSource dataSource = GetIntegratedLocalDataSource();
            Dialogue dialogue = Dummies.Dialogue;
            string relativePath = GetDialoguePath("Local", dialogue);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, dialogue);
            }
            Assert.True(File.Exists(relativePath));
            dataSource.Attach(dialogue);
            TrackedResource<Dialogue> updateTracked = await dataSource.UpdateAsync(dialogue);
            Assert.Equal(ResourceState.Modified, updateTracked.State);
        }
    }
}
