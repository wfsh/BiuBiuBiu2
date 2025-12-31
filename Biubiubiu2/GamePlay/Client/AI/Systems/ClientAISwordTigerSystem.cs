using System;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAISwordTigerSystem : C_AI_Base {
        private GPOM_SwordTiger useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_SwordTiger)MData;
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
            AddComponent<ClientAICatch>();
            AddComponent<ClientAIAnim>();
            AddComponent<ClientAIFootRotation>();
            // 骑乘相关
            AddComponent<ClientAIDrive>();
            AddComponent<ClientAIMoveForDrive>(new ClientAIMoveForDrive.InitData {
                MoveSpeed = useMData.MoveSpeed,
                MoveRotaSpeed = useMData.RotaSpeed,
            });
            AddComponent<ClientSwordTigerAnim>();
        }
    }
}