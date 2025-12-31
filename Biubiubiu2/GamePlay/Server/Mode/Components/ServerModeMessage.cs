using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeMessage : ComponentBase {
        protected override void OnAwake() {
            MsgRegister.Register<SM_Mode.Event_ModeMessage>(OnModeMessageCallBack);
        }

        protected override void OnStart() {
        }

        protected override void OnClear() {
            MsgRegister.Unregister<SM_Mode.Event_ModeMessage>(OnModeMessageCallBack);
        }
        
        private void OnModeMessageCallBack(SM_Mode.Event_ModeMessage ent) {
        }
    }
}