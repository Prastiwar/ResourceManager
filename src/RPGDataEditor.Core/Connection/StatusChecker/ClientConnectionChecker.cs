//using RPGDataEditor.Connection;
//using System.Threading;
//using System.Threading.Tasks;

//namespace RPGDataEditor.Core.Connection
//{
//    public class ClientConnectionChecker : ConnectionChecker
//    {
//        public ClientConnectionChecker(IResourceClient client) => Client = client;

//        public IResourceClient Client { get; }

//        public override async Task<bool> ForceCheckAsync(CancellationToken token)
//        {
//            bool canConnect = await Client.ConnectAsync();
//            if (canConnect)
//            {
//                await Client.DisconnectAsync();
//            }
//            return canConnect;
//        }
//    }
//}
