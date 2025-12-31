using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAISystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }
        protected override void OnClear() {
            base.OnClear();
        }

        private void AddComponents() {
            AddComponent<ServerAIWorld>();
            if (WarReportData.IsStartSausageWarReport == false) {
                if (ModeData.PlayMode == ModeData.ModeEnum.SausageBeastCamp) {
                    AddComponent<ServerCreateBeastCampAI>();
                } else {
                    AddComponent<ServerAICreateForSceneElement>();
                }
            }
        }
    }
}