using System;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIMachineGunSystem : C_AI_Base {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            iEntity.SetPoint(startPoint);
            CreateEntity(AttributeData.SkinSign);
        }
        
        protected override void OnClear() {
            base.OnClear();
        }
        
        protected override void OnLoadEntityEnd(IEntity iEnter) {
            if (iEnter == null) {
                Debug.LogError("[Error] C_Monster_FocusGunSystem 加载 Entity 失败:" + AttributeData.SkinSign);
                return;
            }
        }
        
        
        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ClientAIAttribute>();
            AddComponent<ClientAIMachineGunAttack>();
            AddComponent<ClientAIMachineGunHeadRota>();
            // 骑乘相关
            AddComponent<CliengGPOOtherTeamMaterial>();
            AddComponent<ClientGPOCameraHideRoleRenderer>();
        }
    }
}