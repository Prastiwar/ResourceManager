using ResourceManager.Data;
using System.Collections.Generic;
using Xunit;

namespace ResourceManager.Tests
{
    public class DummyResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }

    public class DescriptorTests
    {
        [Fact]
        public void ParsePathDescriptor()
        {
            PathResourceDescriptor descriptor = new PathResourceDescriptor(typeof(DummyResource), "dummies", "/{category}/{id}_{name}x");
            string path = "dummies/SomeCategory/1_Foox";
            KeyValuePair<string, object>[] parameters = descriptor.ParseParameters(path);
            Assert.Equal(3, parameters.Length);

            Assert.Equal("category", parameters[0].Key);
            Assert.Equal("id", parameters[1].Key);
            Assert.Equal("name", parameters[2].Key);

            Assert.Equal("SomeCategory", parameters[0].Value);
            Assert.Equal("1", parameters[1].Value);
            Assert.Equal("Foo", parameters[2].Value);
        }
    }
}
