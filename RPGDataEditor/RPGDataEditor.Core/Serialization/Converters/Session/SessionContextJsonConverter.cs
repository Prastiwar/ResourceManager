using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGDataEditor.Core.Connection;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Providers;
using System;
using System.Windows;

namespace RPGDataEditor.Core.Serialization
{
    public class SessionContextJsonConverter : ExtendableJsonConverter<DefaultSessionContext>
    {
        public override DefaultSessionContext ReadJObject(Type objectType, JObject obj)
        {
            IResourceClient client = obj.GetValue<IResourceClient>(nameof(ISessionContext.Client));
            OptionsData options = obj.GetValue<OptionsData>(nameof(DefaultSessionContext.Options));
            IClientProvider clientProvider = Application.Current.TryResolve<IClientProvider>();
            DefaultSessionContext session = new DefaultSessionContext() {
                Options = options
            };
            if (clientProvider != null)
            {
                session.ClientProvider = clientProvider;
            }
            if (client != null)
            {
                session.Client = client;
            }
            return session;
        }

        public override JObject ToJObject(DefaultSessionContext value, JsonSerializer serializer) => new JObject() {
                { nameof(DefaultSessionContext.Options).ToLower(), JObject.FromObject(value.Options, serializer) },
                { nameof(ISessionContext.Client).ToLower(), JObject.FromObject(value.Client, serializer) }
            };
    }
}
