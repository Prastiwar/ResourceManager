using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Extensions.Configuration.WriteableJson
{
    public class WritableJsonConfigurationProvider : JsonConfigurationProvider
    {
        public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source) { }

        public override void Set(string key, string value)
        {
            base.Set(key, value);
            string fileFullPath = Source.FileProvider.GetFileInfo(Source.Path).PhysicalPath;
            string json = File.ReadAllText(fileFullPath);
            JObject jsonObj = JObject.Parse(json);
            if (key.Contains(':'))
            {
                string[] keys = key.Split(':');
                JToken currentToken = jsonObj;
                for (int i = 0; i < keys.Length; i++)
                {
                    string currentKey = keys[i];
                    if (currentToken[currentKey] != null)
                    {
                        if (i == keys.Length - 1)
                        {
                            currentToken[currentKey] = value;
                            break;
                        }
                        currentToken = currentToken[currentKey];
                        continue;
                    }
                    JToken valueToken = new JObject(new JProperty(keys[keys.Length - 1], value));
                    int keyIndex = keys.Length - 2;
                    if (i < keyIndex)
                    {
                        JToken nestedObject = new JObject(new JProperty(keys[keyIndex], valueToken));
                        keyIndex--;
                        while (keyIndex > i)
                        {
                            nestedObject = new JObject(new JProperty(keys[keyIndex], nestedObject));
                            keyIndex--;
                        }
                        currentToken[currentKey] = nestedObject;
                    }
                    else
                    {
                        currentToken[currentKey] = valueToken;
                    }
                }
            }
            else
            {
                jsonObj[key] = value;
            }
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fileFullPath, output);
        }
    }
}
