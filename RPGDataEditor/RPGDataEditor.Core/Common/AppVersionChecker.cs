using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RPGDataEditor.Core
{
    public class AppVersionChecker
    {
        public string VersionPath { get; set; }

        public AppVersion ActualVersion { get; set; }

        /// <summary> Sends request to VersionPath to get json file with version </summary>
        /// <returns> True if version is up to date or couldn't check version </returns>
        public async Task<bool> CheckVersionAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                MediaTypeWithQualityHeaderValue mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(mediaType);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, VersionPath);

                HttpResponseMessage response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return true;
                }
                using (Stream s = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                using (StreamReader sr = new StreamReader(s))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializerSettings jsonSettings = JsonConvert.DefaultSettings == null ? null : JsonConvert.DefaultSettings?.Invoke();
                    JsonSerializer serializer = JsonSerializer.Create(jsonSettings);
                    AppVersion value = serializer.Deserialize<AppVersion>(reader);
                    client.Dispose();
                    request.Dispose();
                    response.Dispose();
                    return value.Version == ActualVersion.Version;
                }
            }
            catch (System.Exception ex)
            {
                // most likely there is no internet connection
                return true;
            }

        }
    }
}
