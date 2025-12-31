using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientNetworkManager : ManagerBase {
        private ClientNetworkSystem system;
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<M_Network.SetNetwork>(SetNetwork);
            MsgRegister.Register<M_Network.ClientDisconnect>(OnClientDisconnectCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddNetworkSystem();
        }
        
        private void AddNetworkSystem() {
            if (system != null) {
                return;
            }
            system = AddSystem<ClientNetworkSystem>(null);
        }
        
        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Network.SetNetwork>(SetNetwork);
            MsgRegister.Unregister<M_Network.ClientDisconnect>(OnClientDisconnectCallBack);
            system = null;
        }
        
        private void SetNetwork(M_Network.SetNetwork ent) {
            AddNetworkSystem();
            system.SetNetwork(ent.iNetwork);
        }

        private void OnClientDisconnectCallBack(M_Network.ClientDisconnect ent) {
            Debug.Log("断开服务器链接，返回大厅");
            MsgRegister.Dispatcher(new CM_Game.QuitGame());
        }
    }
}