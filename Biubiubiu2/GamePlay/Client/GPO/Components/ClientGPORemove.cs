using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPORemove : ComponentBase {
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_GPO.Event_StartRemoveGPO>(OnRpcStartRemoveGPOCallBack);
        }

        protected override void OnClear() {
            mySystem.Unregister<CE_GPO.Event_StartRemoveGPO>(OnRpcStartRemoveGPOCallBack);
            base.OnClear();
        }

        private void OnRpcStartRemoveGPOCallBack(ISystemMsg body, CE_GPO.Event_StartRemoveGPO ent) {
            OnRemoveGPO();
        }

        virtual protected void OnRemoveGPO() {
        }
    }
}