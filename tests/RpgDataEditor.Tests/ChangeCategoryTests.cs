using Microsoft.Extensions.Logging;
using Moq;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Mvvm;
using RpgDataEditor.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests
{
    public class ChangeCategoryTests : IntegrationTestClass
    {
        [Fact]
        public Task RenameDialogueCategoryWithFtp()
            => RenameCategoryWithFtp(Dummies.CategoryDialogue, CreateDialogueTabViewModel, dialogue => GetDialoguePath("Ftp", dialogue));

        [Fact]
        public Task RenameDialogueCategoryWithLocal()
            => RenameCategoryWithLocal(Dummies.CategoryDialogue, CreateDialogueTabViewModel, dialogue => GetDialoguePath("Local", dialogue));

        [Fact]
        public Task RenameDialogueCategoryWithSql()
            => RenameCategoryWithSql(Dummies.CategoryDialogue, CreateDialogueTabViewModel);

        [Fact]
        public Task RenameQuestCategoryWithFtp()
            => RenameCategoryWithFtp(Dummies.CategoryQuest, CreateQuestTabViewModel, quest => GetQuestPath("Ftp", quest));

        [Fact]
        public Task RenameQuestCategoryWithLocal()
            => RenameCategoryWithLocal(Dummies.CategoryQuest, CreateQuestTabViewModel, quest => GetQuestPath("Local", quest));

        [Fact]
        public Task RenameQuestCategoryWithSql()
            => RenameCategoryWithSql(Dummies.CategoryQuest, CreateQuestTabViewModel);

        [Fact]
        public Task DeleteDialogueCategoryWithFtp()
            => DeleteCategoryWithFtp(Dummies.CategoryDialogue, CreateDialogueTabViewModel, dialogue => GetDialoguePath("Ftp", dialogue));

        [Fact]
        public Task DeleteDialogueCategoryWithLocal()
            => DeleteCategoryWithLocal(Dummies.CategoryDialogue, CreateDialogueTabViewModel, dialogue => GetDialoguePath("Local", dialogue));

        [Fact]
        public Task DeleteDialogueCategoryWithSql()
            => DeleteCategoryWithSql(Dummies.CategoryDialogue, CreateDialogueTabViewModel);

        [Fact]
        public Task DeleteQuestCategoryWithFtp()
            => DeleteCategoryWithFtp(Dummies.CategoryQuest, CreateQuestTabViewModel, quest => GetQuestPath("Ftp", quest));

        [Fact]
        public Task DeleteQuestCategoryWithLocal()
            => DeleteCategoryWithLocal(Dummies.CategoryQuest, CreateQuestTabViewModel, quest => GetQuestPath("Local", quest));

        [Fact]
        public Task DeleteQuestCategoryWithSql()
            => DeleteCategoryWithSql(Dummies.CategoryQuest, CreateQuestTabViewModel);

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

        protected async Task RenameCategoryWithFtp<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel, Func<TResource, string> getFilePath)
            where TResource : ICategorizable
        {
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    CategoryModelsManagerViewModel<TResource> viewModel = getViewModel.Invoke(dataSource);
                    await viewModel.Refresh();
                    string relativePath = getFilePath(resource);
                    if (!File.Exists(relativePath))
                    {
                        CreateLocalFile(relativePath, resource);
                    }
                    Assert.True(File.Exists(relativePath));
                    string category = resource.Category;
                    string newCategory = Guid.NewGuid().ToString();

                    bool renamed = await viewModel.RenameCategoryAsync(category, newCategory);
                    Assert.True(renamed);
                    Assert.False(File.Exists(relativePath));

                    string newRelativePath = getFilePath(resource);
                    TResource fromFile = GetLocalResource<TResource>(newRelativePath);
                    Assert.NotNull(fromFile);
                    Assert.Equal(newCategory, fromFile.Category);
                }
                catch (Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        protected async Task RenameCategoryWithLocal<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel, Func<TResource, string> getFilePath)
            where TResource : ICategorizable
        {
            IDataSource dataSource = GetIntegratedLocalDataSource();
            CategoryModelsManagerViewModel<TResource> viewModel = getViewModel(dataSource);
            await viewModel.Refresh();
            string relativePath = getFilePath(resource);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, resource);
            }
            Assert.True(File.Exists(relativePath));
            string fromCategory = resource.Category;
            string newCategory = Guid.NewGuid().ToString();

            bool renamed = await viewModel.RenameCategoryAsync(fromCategory, newCategory);
            Assert.True(renamed);
            Assert.False(File.Exists(relativePath));

            string newRelativePath = getFilePath(resource);
            TResource fromFile = GetLocalResource<TResource>(newRelativePath);
            Assert.NotNull(fromFile);
            Assert.Equal(newCategory, fromFile.Category);
        }

        protected async Task RenameCategoryWithSql<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel)
            where TResource : ICategorizable
        {
            IDataSource dataSource = GetIntegratedSqlDataSource();
            bool hasDialogue = dataSource.Query<TResource>().ToList().Contains(resource);
            if (!hasDialogue)
            {
                await dataSource.AddAsync(resource);
                await dataSource.SaveChangesAsync();
                hasDialogue = dataSource.Query<TResource>().ToList().Contains(resource);
                Assert.True(hasDialogue);
            }
            CategoryModelsManagerViewModel<TResource> viewModel = getViewModel(dataSource);
            await viewModel.Refresh();
            string fromCategory = resource.Category;
            string newCategory = Guid.NewGuid().ToString();
            bool renamed = await viewModel.RenameCategoryAsync(fromCategory, newCategory);
            Assert.True(renamed);
            TResource updated = dataSource.Query<TResource>().ToList().First(d => d.Id == resource.Id);
            Assert.Equal(newCategory, updated.Category);
        }

        protected async Task DeleteCategoryWithFtp<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel, Func<TResource, string> getFilePath)
            where TResource : ICategorizable
        {
            using (CancellationTokenSource tokenSource = new CancellationTokenSource())
            {
                try
                {
                    IDataSource dataSource = GetIntegratedFtpDataSource(tokenSource.Token);
                    string relativePath = getFilePath(resource);
                    if (!File.Exists(relativePath))
                    {
                        CreateLocalFile(relativePath, resource);
                    }
                    Assert.True(File.Exists(relativePath));
                    CategoryModelsManagerViewModel<TResource> viewModel = getViewModel(dataSource);
                    await viewModel.Refresh();
                    string category = resource.Category;
                    bool removed = await viewModel.RemoveCategoryAsync(category);
                    Assert.True(removed);
                    Assert.False(File.Exists(relativePath));
                }
                catch (Exception)
                {
                    tokenSource.Cancel();
                    throw;
                }
            }
        }

        protected async Task DeleteCategoryWithLocal<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel, Func<TResource, string> getFilePath)
            where TResource : ICategorizable
        {
            IDataSource dataSource = GetIntegratedLocalDataSource();
            string relativePath = getFilePath(resource);
            if (!File.Exists(relativePath))
            {
                CreateLocalFile(relativePath, resource);
            }
            Assert.True(File.Exists(relativePath));
            CategoryModelsManagerViewModel<TResource> viewModel = getViewModel(dataSource);
            await viewModel.Refresh();
            string category = resource.Category;
            bool removed = await viewModel.RemoveCategoryAsync(category);
            Assert.True(removed);
            Assert.False(File.Exists(relativePath));
        }

        protected async Task DeleteCategoryWithSql<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel)
            where TResource : ICategorizable
        {
            IDataSource dataSource = GetIntegratedSqlDataSource();
            bool hasDialogue = dataSource.Query<TResource>().ToList().Contains(resource);
            if (!hasDialogue)
            {
                await dataSource.AddAsync(resource);
                await dataSource.SaveChangesAsync();
                hasDialogue = dataSource.Query<TResource>().ToList().Contains(resource);
                Assert.True(hasDialogue);
            }
            CategoryModelsManagerViewModel<TResource> viewModel = getViewModel(dataSource);
            await viewModel.Refresh();
            bool removed = await viewModel.RemoveCategoryAsync(resource.Category);
            Assert.True(removed);
            Assert.DoesNotContain(dataSource.Query<TResource>().ToList(), d => EqualityComparer<string>.Default.Equals(d.Category, resource.Category));
        }
    }
}
