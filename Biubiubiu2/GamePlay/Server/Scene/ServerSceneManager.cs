using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSceneManager : ManagerBase {
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Network.SetWorldNetwork>(OnSetWorldNetworkCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Network.SetWorldNetwork>(OnSetWorldNetworkCallBack);
        }

        private void OnSetWorldNetworkCallBack(SM_Network.SetWorldNetwork ent) {
            if (Data.WarReportData.IsStartWarReport()) {
                return;
            }
            AddSystem(delegate(ServerSceneSystem monsterSystem) {
                monsterSystem.SetNetwork(ent.network);
            });
        }
    }
}