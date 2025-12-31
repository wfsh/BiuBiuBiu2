using System;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIHelicopterSystem : C_AI_Base {
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
        
        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ClientAIAttribute>();
            AddComponent<ClientAIHelicopterMoveForDrive>();
            AddComponent<ClientAIHelicopterAttack>();
            AddComponent<ClientAIHelicopterRootBody>();
            AddComponent<ClientAIHelicopterPropellerRotatio>();
            // 骑乘相关
            AddComponent<ClientAIDrive>();
            AddComponent<CliengGPOOtherTeamMaterial>();
            AddComponent<ClientGPOCameraHideRoleRenderer>();
        }
    }
}