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
    public class FtpChangeCategoryTests : FtpIntegrationTestClass
    {
        [Fact]
        public Task RenameDialogueCategory()
            => RenameCategory(Dummies.CategoryDialogue, CreateDialogueTabViewModel, dialogue => GetDialoguePath("Ftp", dialogue));

        [Fact]
        public Task RenameQuestCategory()
            => RenameCategory(Dummies.CategoryQuest, CreateQuestTabViewModel, quest => GetQuestPath("Ftp", quest));

        [Fact]
        public Task DeleteDialogueCategory()
            => DeleteCategory(Dummies.CategoryDialogue, CreateDialogueTabViewModel, dialogue => GetDialoguePath("Ftp", dialogue));

        [Fact]
        public Task DeleteQuestCategory()
            => DeleteCategory(Dummies.CategoryQuest, CreateQuestTabViewModel, quest => GetQuestPath("Ftp", quest));

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

        protected async Task RenameCategory<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel, Func<TResource, string> getFilePath)
            where TResource : ICategorizable
        {
            IDataSource dataSource = ConnectDataSource();
            CategoryModelsManagerViewModel<TResource> viewModel = getViewModel.Invoke(dataSource);
            string relativePath = getFilePath(resource);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, resource);
            }
            Assert.True(File.Exists(relativePath));
            string category = resource.Category;
            string newCategory = Guid.NewGuid().ToString();

            await viewModel.Refresh();
            bool renamed = await viewModel.RenameCategoryAsync(category, newCategory);
            Assert.True(renamed);
            Assert.False(File.Exists(relativePath));

            TResource newResource = viewModel.Models.First(r => IdentifiableComparer.Default.Compare(r, resource) == 0);
            string newRelativePath = getFilePath(newResource);
            TResource fromFile = GetLocalResource<TResource>(newRelativePath);
            Assert.NotNull(fromFile);
            Assert.Equal(newCategory, fromFile.Category);
        }

        protected async Task DeleteCategory<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel, Func<TResource, string> getFilePath)
            where TResource : ICategorizable
        {
            IDataSource dataSource = ConnectDataSource();
            string relativePath = getFilePath(resource);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, resource);
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
