using System;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIJokerUavSystem : C_AI_Base {
        public GPOM_JokerUav useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_JokerUav)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            iEntity.SetPoint(startPoint);
            iEntity.SetRota(startRot);
            CreateEntityToPool(AISkinSign);
        }

        protected override void OnClear() {
            base.OnClear();
        }
        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<ClientAIAttribute>();
            AddComponent<ClientAIJokerUAVState>();
            AddComponent<ClientAIAnim>(new ClientAIAnim.InitData() {
                ConfigSign = AIAnimConfig.GoldDash_JokerDrone,
            });
        }
    }
}