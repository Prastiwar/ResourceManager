using ResourceManager.DataSource;
using RpgDataEditor.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Local
{
    public class TrackingTests
    {
        [Fact]
        public async Task ThrowOnUpdateNoTracking()
        {
            using (LocalIntegrationTestProvider integration = new LocalIntegrationTestProvider())
            {
                IDataSource dataSource = integration.ConnectDataSource();
                Dialogue dialogue = Dummies.Dialogue;
                string relativePath = integration.GetDialoguePath(dialogue);
                if (!File.Exists(relativePath))
                {
                    integration.CreateLocalFile(relativePath, dialogue);
                }
                Assert.True(File.Exists(relativePath));

                await Assert.ThrowsAsync<InvalidOperationException>(() => dataSource.UpdateAsync(dialogue));
            }
        }

        [Fact]
        public async Task DontThrowOnUpdateTracking()
        {
            using (LocalIntegrationTestProvider integration = new LocalIntegrationTestProvider())
            {
                IDataSource dataSource = integration.ConnectDataSource();
                Dialogue dialogue = Dummies.Dialogue;
                string relativePath = integration.GetDialoguePath(dialogue);
                if (!File.Exists(relativePath))
                {
                    integration.CreateLocalFile(relativePath, dialogue);
                }
                Assert.True(File.Exists(relativePath));
                dataSource.Attach(dialogue);
                TrackedResource<Dialogue> updateTracked = await dataSource.UpdateAsync(dialogue);
                Assert.Equal(ResourceState.Modified, updateTracked.State);
            }
        }
    }
}
