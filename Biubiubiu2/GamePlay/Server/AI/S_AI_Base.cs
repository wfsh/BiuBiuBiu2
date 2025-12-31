using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class S_AI_Base : S_GPO_Base, IAI {
        public string AISkinSign {get; private set;}
        // 主人 GPO
        public IGPO MasterGPO { get; private set; }

        /// <summary>
        /// 所有可以交互的 GPO 列表
        /// </summary>
        public List<IGPO> GPOList { get; private set; }

        public IGPOM MData;
        public IGPOInData InData;
        private ITargetRpc protoDoc;
        private string poolUrl = "";
        private bool isUsePoolEntity = false;


        protected override void OnClearBase() {
            base.OnClearBase();
            GPOList = null;
        }

        protected override void OnAwakeBase() {
            base.OnAwakeBase();
            AddNetwork();
        }
        
        override protected void OnSetAttributeData(GPOData.AttributeData attributeData) {
            attributeData.SkinSign = this.AISkinSign;
        }

        public void SetAIData(IGPOM mData, IGPOInData inData, int gpoId, int teamId, GPOData.GPOType gpoType, string skinSign, IGPO masterGPO) {
            this.MData = mData;
            this.InData = inData;
            this.MasterGPO = masterGPO;
            this.AISkinSign = skinSign;
            SetGPOData(gpoId, gpoType, mData, inData,teamId);
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = OnGetGPOListCallBack
            });
        }

        public void SetStartPoint(Vector3 point) {
            iEntity.SetPoint(point);
            Dispatcher(new SE_AI.Event_SetStartPoint {
                StartPoint = point,
            });
        }

        public void SetStartRota(Quaternion rota) {
            if (rota == Quaternion.identity) {
                return;
            }
            iEntity.SetRota(rota);
        }

        public void CreateEntity(string sign) {
            if (sign == "") {
                Debug.LogError($"[Error] S_AI_Base.CreateEntity sign 为空  [{GetType().Name}]");
                SetIEntity(null);
                return;
            }
            CreateEntityObj(string.Concat("AI/Server/", sign), StageData.GameWorldLayerType.AI);
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
            poolUrl = $"{AssetURL.GamePlay}/AI/Server/{sign}.prefab";
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
                    entityBase.gameObject.name = $"[Server]{this.AISkinSign}";
                }
                PrefabPoolManager.OnReturnPrefab(poolUrl, entityBase.gameObject);
            }
        }
        
        private void OnGetGPOListCallBack(List<IGPO> list) {
            GPOList = list;
        }

        public GPOData.AttributeData GetAttributeData() {
            return AttributeData;
        }
        
        public IGPO GetMasterGPO() {
            return MasterGPO;
        } 

        public override NetworkData.SpawnConnType GetSpawnConnType() {
            return NetworkData.SpawnConnType.AI;
        }

        private void AddNetwork() {
            // 网络同步组件
            AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            // 移动同步组件
            AddComponent<ServerNetworkTransform>();
        }

        void OnSyncSpawnProto(ServerNetworkSync sync) {
            sync.SetSpawnRPC(RpcAI(sync));
        }

        /// <summary>
        /// 初始化客户端使用的数据，每个玩家连接都会自动下发
        /// </summary>
        /// <returns></returns>
        virtual protected ITargetRpc RpcAI(ServerNetworkSync sync) {
            if (GpoID <= 0) {
                Debug.LogError("S_AI_Base 下发的 AI 对象 GPO ID 不能 <= 0 MID:" + MData.GetId() + " SkinSign:" + AISkinSign);
                return null;
            }
            var protoDoc = Array.Empty<byte>();
            if (AttributeData != null) {
                protoDoc = sync.SerializeProto(new Proto_AI.TargetRpc_AddAIDefault {
                    maxHp = AttributeData.maxHp,
                    nowHp = AttributeData.nowHp,
                });
            }
            return new Proto_AI.TargetRpc_AddAI() {
                gpoId = GpoID,
                teamId = TeamId,
                gpoMId = (ushort)MData.GetId(),
                aiSkinSign = AISkinSign,
                startPoint = iEntity.GetPoint(),
                startRota = iEntity.GetRota(),
                protoDoc = protoDoc
            };
        }

        /// <summary>
        /// 基础的组件集合
        /// </summary>
        virtual protected void AddComponents() {
            AddComponent<ServerAIQuality>(); 
            AddComponent<ServerAIMaster>();
            AddComponent<KnockbackGPO>();
            AddComponent<StrikeFlyGPO>();
            AddComponent<ServerGPOIsGodMode>(); // 是否无敌模式
            AddComponent<ServerGPOShowEntity>();  // 怪物 GameOBJ 显示隐藏
            AddComponent<ServerGPOAbilityEffect>();  // 怪物技能 (BUFF)特效
            AddComponent<ServerGPODropItem>();
            if (ModeData.PlayMode == ModeData.ModeEnum.SausageGoldDash || 
                ModeData.PlayMode == ModeData.ModeEnum.SausageBeastCamp) { 
                AddComponent<ServerGoldDashAIPatrolPoint>(); // 巡逻点
                AddComponent<ServerGoldDashAIHurt>();  // 怪物受伤
                AddComponent<ServerGoldDashAIDead>();  // 怪物死亡组件
                AddComponent<ServerAIToSausageWarReport>();   // 香肠战报通信组件
                AddComponent<ServerAIToSausage>(); 
                AddComponent<ServerGPOMat>();
            } else {
                AddComponent<ServerAIPatrolPoint>();
                AddComponent<ServerAIHurt>();
                AddComponent<ServerAIDead>();
            }
        }
    }
}