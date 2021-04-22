using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ResourceManager;
using RPGDataEditor.Core.Connection;
using System;

namespace RPGDataEditor.Core.Serialization
{
    public class SqlClientJsonConverter : ExtendableJsonConverter<ISqlClient>
    {
        public override ISqlClient ReadJObject(Type objectType, JObject obj)
        {
            string type = obj.GetValue<string>("type");
            ISqlClient client = CreateClient(type);
            switch (client)
            {
                case SqlClient sqlClient:
                    sqlClient.ConnectionString = obj.GetValue<string>(nameof(SqlClient.ConnectionString));
                    break;
                default:
                    break;
            }
            return client;
        }

        protected virtual ISqlClient CreateClient(string type) => type.ToLower() switch {
            "sql" => new SqlClient(),
            _ => null,
        };

        public override JObject ToJObject(ISqlClient value, JsonSerializer serializer) => value switch {
            SqlClient sqlClient => new JObject() {
                    { "type", "sql" },
                    { nameof(SqlClient.ConnectionString).ToLower(), sqlClient.ConnectionString }
                },
            _ => new JObject(value)
        };
    }

}
