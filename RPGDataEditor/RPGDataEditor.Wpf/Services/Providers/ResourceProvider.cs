using RPGDataEditor.Core;
using RPGDataEditor.Core.Models;
using RPGDataEditor.Core.Mvvm;
using RPGDataEditor.Core.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPGDataEditor.Wpf.Providers
{
    public class ResourceProvider : IResourceProvider
    {
        public ResourceProvider(SessionContext session) => Session = session;

        protected SessionContext Session { get; }

        public async Task<IIdentifiable[]> LoadResourcesAsync(int resource)
        {
            List<IIdentifiable> list = new List<IIdentifiable>();
            switch (resource)
            {
                case (int)RPGResource.Quest:
                    list = new List<IIdentifiable>(await Session.LoadQuests());
                    break;
                case (int)RPGResource.Dialogue:
                    list = new List<IIdentifiable>(await Session.LoadDialogues());
                    break;
                case (int)RPGResource.Npc:
                    list = new List<IIdentifiable>(await Session.LoadNpcs());
                    break;
                default:
                    break;
            }
            return list.ToArray();
        }
    }
}