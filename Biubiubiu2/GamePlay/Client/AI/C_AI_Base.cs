using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class C_AI_Base : C_GPO_Base, IAI {
        protected Vector3 startPoint = Vector3.zero;
        protected Quaternion startRot = Quaternion.identity;
        public string AISkinSign {get; private set;}
        private string poolUrl = "";
        private bool isUsePoolEntity = false;

        // 主人 GPO
        public IGPO MasterGPO { get; private set; }

        protected override void OnAwakeBase() {
            base.OnAwakeBase();
        }

        protected override void OnClearBase() {
            base.OnClearBase();
            MasterGPO = null;
        }

        public void SetAIData(IGPOM gpoMData, IProto_Doc inData, int gpoId, int teamId, string skinSign, Vector3 startPoint, Quaternion startRot) {
            this.AISkinSign = skinSign;
            this.startPoint = startPoint;
            this.startRot = startRot;
            iEntity.SetPoint(startPoint);
            iEntity.SetRota(startRot);
            SetGPOData(gpoId, gpoMData, GPOData.GPOType.AI, inData, teamId, false);
            AddComponent<ClientWorldNetworkBehaviour>();
        }

        
        protected override void OnSetAttributeData(GPOData.AttributeData attributeData) {
            attributeData.SkinSign = this.AISkinSign;
        }

        public void CreateEntity(string sign) {
            if (sign == "") {
                Debug.LogError($"[Error] C_AI_Base.CreateEntity sign 为空  [{GetType().Name}]");
                SetIEntity(null);
                return;
            }
            CreateEntityObj(string.Concat("AI/Client/", sign), StageData.GameWorldLayerType.AI);
        }

        /// <summary>
        /// 使用缓冲池的方式创建实体
        /// </summary>
        /// <param name="sign"></param>
        public void CreateEntityToPool(string sign) {
            if (sign == "") {
                Debug.LogError($"[Error] C_AI_Base.CreateEntityToPool sign 为空  [{GetType().Name}]");
                SetIEntity(null);
                return;
            }
            poolUrl = $"{AssetURL.GamePlay}/AI/Client/{sign}.prefab";
            PrefabPoolManager.OnGetPrefab(poolUrl,
                null,
                gameObj => {
                    if (IsClear()) {
                        PrefabPoolManager.OnReturnPrefab(poolUrl, gameObj);
                        return;
                    }
                    isUsePoolEntity = true;
                    SetEntity(gameObj, StageData.GameWorldLayerType.AI);
                });
            PrefabPoolManager.OnSetInactiveTime(poolUrl, -1);
        }

        protected override void OnRemoveEntity() {
            base.OnRemoveEntity();
            if (isUsePoolEntity) {
                var entityBase = (EntityBase)iEntity;
                if (Application.isEditor) {
                    entityBase.gameObject.name = $"[Client]{this.AISkinSign}";
                }
                PrefabPoolManager.OnReturnPrefab(poolUrl, entityBase.gameObject);
            }
        }

        public void SetMasterGPO(IGPO masterGPO) {
            this.MasterGPO = masterGPO;
        }
        
        public GPOData.AttributeData GetAttributeData() {
            return AttributeData;
        }
        
        public IGPO GetMasterGPO() {
            return MasterGPO;
        }

        virtual protected void AddComponents() {
            AddComponent<ClientAIQuality>();
            AddComponent<ClientNetworkTransform>();
            AddComponent<ClientGPODead>();
            AddComponent<ClientGPOGodMode>();
            AddComponent<ClientGPOShowEntity>();
            AddComponent<ClientAIRemove>();
            AddComponent<ClientAIMaster>();
            AddComponent<ClientGPOAbilityEffect>();
            AddComponent<ClientAIEffect>();
            AddComponent<CliengGPOMat>();
        }
    }
}