using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeHandlerErrorCheck : ComponentBase {
        private float maxHandlerUseTime = 0.0f;
        protected override void OnAwake() {
            mySystem.Register<SE_Mode.Event_GameState>(OnSetGameStateCallBack);
            MsgRegister.Register<M_Game.HandlerErrorUseTime>(OnHandlerErrorUseTime);
        }
    
        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Mode.Event_GameState>(OnSetGameStateCallBack);
            MsgRegister.Unregister<M_Game.HandlerErrorUseTime>(OnHandlerErrorUseTime);
        }

        private void OnSetGameStateCallBack(ISystemMsg body, SE_Mode.Event_GameState ent) {
            if (ent.GameState == ModeData.GameStateEnum.QuitApp) {
                var kcp = NetworkData.Config.IsKCP ? "KCP" : "WEB";
                if (maxHandlerUseTime > 1000f) { 
                    Debug.LogError($"{kcp} SystemBase 耗时过长 {maxHandlerUseTime}");
                } else if (MsgRegister.MaxHandlerUseTime > 1000f) {
                    Debug.LogError($"{kcp} MsgRegister 耗时过长 {MsgRegister.MaxHandlerUseTime}");
                }
            }
        }
        
        private void OnHandlerErrorUseTime(M_Game.HandlerErrorUseTime ent) {
            if (ent.UseTime > maxHandlerUseTime) {
                maxHandlerUseTime = ent.UseTime;
            }
        }
    }
}