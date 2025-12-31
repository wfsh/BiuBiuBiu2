using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerNetworkManager : ManagerBase {
        protected override void OnAwake() {
            base.OnAwake();
            NetworkData.ConnIndex = 0;
            AddSystem<ServerNetworkSystem>(null);
        }
    }
}