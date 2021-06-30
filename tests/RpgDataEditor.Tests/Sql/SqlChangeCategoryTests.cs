using Microsoft.Extensions.Logging;
using Moq;
using ResourceManager;
using ResourceManager.Data;
using ResourceManager.DataSource;
using ResourceManager.Mvvm;
using RpgDataEditor.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RpgDataEditor.Tests.Sql
{
    public class SqlChangeCategoryTests
    {
        [Fact]
        public Task RenameDialogueCategory()
            => RenameCategory(Dummies.CategoryDialogue, CreateDialogueTabViewModel);

        [Fact]
        public Task RenameQuestCategory()
            => RenameCategory(Dummies.CategoryQuest, CreateQuestTabViewModel);

        [Fact]
        public Task DeleteDialogueCategory()
            => DeleteCategory(Dummies.CategoryDialogue, CreateDialogueTabViewModel);

        [Fact]
        public Task DeleteQuestCategory()
            => DeleteCategory(Dummies.CategoryQuest, CreateQuestTabViewModel);

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

        protected async Task RenameCategory<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel)
            where TResource : ICategorizable
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                IDataSource dataSource = integration.ConnectDataSource();
                bool hasDialogue = dataSource.Query<TResource>().ToList().Contains(resource);
                if (!hasDialogue)
                {
                    await dataSource.AddAsync(resource);
                    await dataSource.SaveChangesAsync();
                    hasDialogue = dataSource.Query<TResource>().ToList().Contains(resource);
                    Assert.True(hasDialogue);
                }
                CategoryModelsManagerViewModel<TResource> viewModel = getViewModel(dataSource);
                string fromCategory = resource.Category;
                string newCategory = Guid.NewGuid().ToString();
                await viewModel.Refresh();
                bool renamed = await viewModel.RenameCategoryAsync(fromCategory, newCategory);
                Assert.True(renamed);
                TResource updated = dataSource.Query<TResource>().ToList().First(d => IdentifiableComparer.Default.Compare(d, resource) == 0);
                Assert.Equal(newCategory, updated.Category);
            }
        }

        protected async Task DeleteCategory<TResource>(TResource resource, Func<IDataSource, CategoryModelsManagerViewModel<TResource>> getViewModel)
            where TResource : ICategorizable
        {
            using (SqlIntegrationTestProvider integration = new SqlIntegrationTestProvider())
            {
                IDataSource dataSource = integration.ConnectDataSource();
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
}
