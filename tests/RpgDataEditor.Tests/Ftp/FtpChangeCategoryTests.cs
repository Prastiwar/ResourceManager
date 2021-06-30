using Microsoft.Extensions.Logging;
using Moq;
using ResourceManager;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Mvvm;
using RpgDataEditor.Wpf.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Ftp
{
    [Collection(NonParallelCollectionDefinition.NAME)]
    public class FtpChangeCategoryTests
    {
        [Fact]
        public Task RenameDialogueCategory()
            => RenameCategory(Dummies.CategoryDialogue, CreateDialogueTabViewModel, (integration, dialogue) => integration.GetDialoguePath(dialogue));

        [Fact]
        public Task RenameQuestCategory()
            => RenameCategory(Dummies.CategoryQuest, CreateQuestTabViewModel, (integration, quest) => integration.GetQuestPath(quest));

        [Fact]
        public Task DeleteDialogueCategory()
            => DeleteCategory(Dummies.CategoryDialogue, CreateDialogueTabViewModel, (integration, dialogue) => integration.GetDialoguePath(dialogue));

        [Fact]
        public Task DeleteQuestCategory()
            => DeleteCategory(Dummies.CategoryQuest, CreateQuestTabViewModel, (integration, quest) => integration.GetQuestPath(quest));

        protected DialogueTabViewModel CreateDialogueTabViewModel(IDataSource dataSource)
        {
            Mock<ILogger<DialogueTabViewModel>> logger = new Mock<ILogger<DialogueTabViewModel>>();
            return new DialogueTabViewModel(null, dataSource, logger.Object);
        }
        protected QuestTabViewModel CreateQuestTabViewModel(IDataSource dataSource)
        {
            Mock<ILogger<QuestTabViewModel>> logger = new Mock<ILogger<QuestTabViewModel>>();
            return new QuestTabViewModel(null, dataSource, logger.Object);
        }

        protected async Task RenameCategory<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel, Func<FtpIntegrationTestProvider, TResource, string> getFilePath)
            where TResource : ICategorizable
        {
            using (FtpIntegrationTestProvider integration = new FtpIntegrationTestProvider())
            {
                IDataSource dataSource = integration.ConnectDataSource();
                CategoryModelsManagerViewModel<TResource> viewModel = getViewModel.Invoke(dataSource);
                string relativePath = getFilePath(integration, resource);
                if (!File.Exists(relativePath))
                {
                    integration.CreateLocalFile(relativePath, resource);
                }
                Assert.True(File.Exists(relativePath));
                string category = resource.Category;
                string newCategory = Guid.NewGuid().ToString();

                await viewModel.Refresh();
                bool renamed = await viewModel.RenameCategoryAsync(category, newCategory);
                Assert.True(renamed);
                Assert.False(File.Exists(relativePath));

                TResource newResource = viewModel.Models.First(r => IdentifiableComparer.Default.Compare(r, resource) == 0);
                string newRelativePath = getFilePath(integration, newResource);
                TResource fromFile = integration.GetLocalResource<TResource>(newRelativePath);
                Assert.NotNull(fromFile);
                Assert.Equal(newCategory, fromFile.Category);
            }
        }

        protected async Task DeleteCategory<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel, Func<FtpIntegrationTestProvider, TResource, string> getFilePath)
            where TResource : ICategorizable
        {
            using (FtpIntegrationTestProvider integration = new FtpIntegrationTestProvider())
            {
                IDataSource dataSource = integration.ConnectDataSource();
                string relativePath = getFilePath(integration, resource);
                if (!File.Exists(relativePath))
                {
                    integration.CreateLocalFile(relativePath, resource);
                }
                Assert.True(File.Exists(relativePath));
                CategoryModelsManagerViewModel<TResource> viewModel = getViewModel(dataSource);
                string category = resource.Category;
                await viewModel.Refresh();
                bool removed = await viewModel.RemoveCategoryAsync(category);
                Assert.True(removed);
                Assert.False(File.Exists(relativePath));
            }
        }
    }
}
