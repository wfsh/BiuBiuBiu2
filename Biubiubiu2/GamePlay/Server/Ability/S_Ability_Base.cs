using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class S_Ability_Base : SystemBase, IAbilitySystem {
        public int AbilityId { get; private set; }
        public string AbilityTypeID { get; private set; }
        public ushort ConfigID { get; private set; }
        public byte RowId { get; private set; }
        public ServerGPO FireGPO { get; private set; }
        public ServerGPO TargetGPO { get; private set; }
        public IAbilitySystem ParentAB { get; private set; }

        /// <summary>
        /// 所有可以交互的 GPO 列表
        /// </summary>
        public List<IGPO> GPOList { get; private set; }

        // 传入的数据
        public IAbilityMData MData = null;
        public IAbilityInData InData = null;

        protected override void OnClearBase() {
            base.OnClearBase();
            this.GPOList = null;
            this.MData = null;
            this.InData = null;
        }
        public void SetData(int abilityId, IAbilityMData mData, IAbilityInData inData) {
            this.AbilityId = abilityId;
            this.InData = inData;
            this.MData = mData;
            this.RowId = mData.GetRowID();
            this.ConfigID = mData.GetConfigId();
            this.AbilityTypeID = mData.GetTypeID();
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = OnGetGPOListCallBack
            });
        }

        public void SetFireGPO(IGPO igpo) {
            FireGPO = (ServerGPO)igpo;
        }

        public void SetTargetGPO(IGPO igpo) {
            TargetGPO = (ServerGPO)igpo;
        }

        public void SetParentAB(IAbilitySystem ab) {
            ParentAB = ab;
        }

        private void OnGetGPOListCallBack(List<IGPO> list) {
            GPOList = list;
        }

        public void ResetEffect() {
            OnResetEffect();
            Dispatcher(new SE_AbilityEffect.Event_ResetAbilityEffect());
        }
        
        virtual protected void OnResetEffect() {
        }

        public void CreateEntity(string sign) {
            if (string.IsNullOrEmpty(sign)) {
                SetIEntity(null);
                return;
            }
            CreateEntityObj(string.Concat("Ability/", sign), StageData.GameWorldLayerType.Ability);
        }

        public int GetAbilityId() {
            return this.AbilityId;
        }

        public IGPO GetFireGPO() {
            return FireGPO;
        }

        public Vector3 GetPoint() {
            return iEntity.GetPoint();
        }

        public override NetworkData.SpawnConnType GetSpawnConnType() {
            return NetworkData.SpawnConnType.Ability;
        }
        
        virtual protected void AddComponents() {
            AddComponent<ServerAbilitySync>(); // 服务器同步组件 （简单的进行同步）
        }

        /// <summary>
        ///  RPC 下发数据 需要和 ServerAbilitySync 配套
        ///  如果要走常驻的 AB 不用走这边的 RPC ，需要单独添加 ServerNetworkSync 组件，可以参考 SAB_TestFire 的写法
        /// </summary>
        protected void RPCAbility(IProto_Doc protoDoc) {
            Dispatcher(new SE_Ability.RPCAbility {
                ProtoData = protoDoc
            });
        }

        protected void EnableRPCRemoveAbility() {
            Dispatcher(new SE_Ability.EnableRPCRemoveAbility());
        }
    }
}