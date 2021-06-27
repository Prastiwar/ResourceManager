using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xunit;

namespace Extensions.Configuration.WriteableJson.Tests
{
    public class WritingTests
    {
        protected IDisposable TestConfiguration(out IConfiguration configuration, out string filePath)
        {
            filePath = "WritingTestsFile.json";
            int counter = 1;
            while (File.Exists(filePath))
            {
                filePath = $"WritingTestsFile ({counter}).json";
            }
            File.WriteAllText(filePath, "{\"ExistingProperty\":\"ExistingValue\"}");
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddWriteableJsonFile(filePath);
            configuration = builder.Build();
            string pathToDelete = filePath;
            return new DisposableTrigger(() => File.Delete(pathToDelete));
        }

        protected void AssertExistingValuesDontBreak(JObject fileObject) => Assert.Equal("ExistingValue", fileObject["ExistingProperty"]);

        protected JObject ReadConfiguration(string filePath) => JObject.Parse(File.ReadAllText(filePath));

        [Fact]
        public void SetValue()
        {
            using (TestConfiguration(out IConfiguration configuration, out string filePath))
            {
                configuration["Property"] = "Value";
                JObject fileObject = ReadConfiguration(filePath);
                Assert.True(fileObject.HasValues);
                Assert.Equal("Value", fileObject["Property"]);
                AssertExistingValuesDontBreak(fileObject);
            }
        }

        [Fact]
        public void SetNestedValue()
        {
            using (TestConfiguration(out IConfiguration configuration, out string filePath))
            {
                configuration["Nested:Property"] = "Value";
                JObject fileObject = ReadConfiguration(filePath);

                Assert.True(fileObject.HasValues);
                Assert.NotNull(fileObject["Nested"]);
                Assert.Equal("Value", fileObject["Nested"]["Property"]);
                AssertExistingValuesDontBreak(fileObject);
            }
        }

        [Fact]
        public void SetDeepNestedValue()
        {
            using (TestConfiguration(out IConfiguration configuration, out string filePath))
            {
                configuration["Deep:Nested:Property"] = "Value";
                JObject fileObject = ReadConfiguration(filePath);

                Assert.True(fileObject.HasValues);
                Assert.NotNull(fileObject["Deep"]);
                Assert.NotNull(fileObject["Deep"]["Nested"]);
                Assert.Equal("Value", fileObject["Deep"]["Nested"]["Property"]);
                AssertExistingValuesDontBreak(fileObject);
            }
        }
    }
}
