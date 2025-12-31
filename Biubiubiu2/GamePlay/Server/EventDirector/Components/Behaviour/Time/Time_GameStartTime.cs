using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Time_GameStartTime : EventDirectorTime {
        private float gamePlayTime = 0f;
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Mode.UpdateGameTime>(OnUpdateGameTime);
        }
        
        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Mode.UpdateGameTime>(OnUpdateGameTime);
        }
        
        private void OnUpdateGameTime(SM_Mode.UpdateGameTime ent) {
            gamePlayTime = ent.GameTime;
        }
        
        public override bool CheckInTime() {
            if (EndTime > 0) {
                if (gamePlayTime >= StartTime && gamePlayTime <= EndTime) {
                    return true;
                }
            } else {
                if (gamePlayTime >= StartTime) {
                    return true;
                }
            }
            return false;
        }
    }
}
