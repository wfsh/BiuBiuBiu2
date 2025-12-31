using System;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIAuroraDragonSystem : C_AI_Base {
        public GPOM_AuroraDragon useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_AuroraDragon)MData;
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

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ClientAIAttribute>();
            AddComponent<ClientAIAnim>();
            AddComponent<ClientTargetAuroraDragonFightRange>();
        }
    }
}