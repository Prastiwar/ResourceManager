using Xunit;

namespace RpgDataEditor.Tests
{
    [CollectionDefinition(NAME, DisableParallelization = true)]
    public class NonParallelCollectionDefinition
    {
        public const string NAME = nameof(NonParallelCollectionDefinition);
    }
}
