
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class WorldSync : SyncBase, IWorldSync {
        private INetwork network;
        public void SetNetwork(INetwork network) {
            this.network = network;
        }
    }
}