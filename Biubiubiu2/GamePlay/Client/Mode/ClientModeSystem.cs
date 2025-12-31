using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientModeSystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }
        protected override void OnClear() {
            base.OnClear();
        }

        private void AddComponents() {
            if (ModeData.PlayMode == ModeData.ModeEnum.None) {
                return;
            }
            if (ModeData.IsSausageMode() == false) {
                AddComponent<ClientModeMainLoop>();
            }
            switch (ModeData.PlayMode) {
                case ModeData.ModeEnum.ModeExplore:
                    AddComponent<ClientSaveBattleData>();
                    break;
                case ModeData.ModeEnum.ModeBoss:
                    AddComponent<ClientGetSaveBattleData>();
                    break;
                case ModeData.ModeEnum.Mode1V1:
                    AddComponent<ClientVSMode>();
                    break;
            }
        }
    }
}