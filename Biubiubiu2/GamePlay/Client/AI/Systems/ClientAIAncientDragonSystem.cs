using System;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIAncientDragonSystem : C_AI_Base {
        private GPOM_AuroraDragon useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_AuroraDragon)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            iEntity.SetPoint(startPoint);
            iEntity.SetRota(startRot);
            CreateEntity(AISkinSign);
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            if (iEnter == null) {
                Debug.LogError("[Error] C_Monster_FocusGunSystem 加载 Entity 失败:" + AISkinSign);
                return;
            }
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ClientAIAttribute>();
            AddComponent<ClientAIAnim>(new ClientAIAnim.InitData {
                ConfigSign = "AuroraDragon",
                ChangeAssetUrl = "AuroraDragon",
                ToAssetUrl = "AncientDragon",
            });
            AddComponent<ClientTargetAuroraDragonFightRange>();
        }
    }
}