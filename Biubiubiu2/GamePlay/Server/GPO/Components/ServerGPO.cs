using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    /// <summary>
    /// 玩法互动的关键组件
    /// </summary>
    public class ServerGPO : ServerNetworkComponentBase, IGPO {
        public class InitData : SystemBase.IComponentInitData {
            public GPOData.GPOType GpoType;
            public IGPOM GpoMData;
            public int TeamId;
            public int SetGpoID;
        }
        private GPOData.GPOType gpoType = GPOData.GPOType.NULL;
        private bool isGodMode = false;
        private bool isDead = false;
        private int gpoID = 0;
        private int teamId = 0;
        private bool isOnline = true;
        private Vector3 point = Vector3.zero;
        private IGPOM gpoMData = null;
        private S_GPO_Base gpoSystem;
        private EntityBase gpoEntity;

        protected override void OnAwake() {
            gpoSystem = (S_GPO_Base)mySystem;
            mySystem.Register<SE_GPO.Event_IsGodMode>(OnIsGodModeCallBack);
            mySystem.Register<SE_GPO.Event_StartRemoveGPO>(OnSetRemoveGPOCallBack);
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Register<SE_Network.Event_SetIsOnline>(OnSetIsOnlineCallBack);
            mySystem.Register<SE_Network.Event_GetIsOnline>(OnGetIsOnlineCallBack);
            var initData = (InitData)initDataBase;
            this.gpoMData = initData.GpoMData;
            this.gpoType = initData.GpoType;
            this.teamId = initData.TeamId;
            if (initData.SetGpoID != 0) {
                this.gpoID = initData.SetGpoID;
            } else {
                gpoID = ++GPOData.GPOIndex;
            }
            SetSign(this.gpoMData.GetSign());
            this.mySystem.SetIGPO(this);
        }

        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Dispatcher(new SM_GPO.AddGPO {
                IGpo = this
            });
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            gpoEntity = (EntityBase)iEntity;
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_IsGodMode>(OnIsGodModeCallBack);
            mySystem.Unregister<SE_GPO.Event_StartRemoveGPO>(OnSetRemoveGPOCallBack);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Unregister<SE_Network.Event_SetIsOnline>(OnSetIsOnlineCallBack);
            mySystem.Unregister<SE_Network.Event_GetIsOnline>(OnGetIsOnlineCallBack);
            MsgRegister.Dispatcher(new SM_GPO.RemoveGPO {
                GpoId = gpoID
            });
            gpoMData = null;
            gpoSystem = null;
            gpoEntity = null;
        }

        public IGPOM GetMData() {
            return this.gpoMData;
        }

        protected override ITargetRpc SyncData() {
            return new Proto_GPO.TargetRpc_SyncGPOID {
                gpoID = gpoID, teamID = teamId, gpoType = gpoType,
            };
        }
        
        public int GetGpoMID() {
            if (this.gpoMData == null) {
                return 0;
            }
            return this.gpoMData.GetId();
        }
        public int GetGpoMTypeID() {
            if (this.gpoMData == null) {
                return 0;
            }
            return this.gpoMData.GetGpoType();
        }
        
        public GPOData.AttributeData GetAttributeData() {
            if (gpoSystem == null) {
                return null;
            }
            return gpoSystem.AttributeData;
        }

        public int GetGpoID() {
            return gpoID;
        }

        public int GetTeamID() {
            return teamId;
        }

        public Vector3 GetPoint() {
            if (isClear) {
                return point;
            }
            point = iEntity.GetPoint();
            return point;
        }

        public Quaternion GetRota() {
            if (isClear) {
                return Quaternion.identity;
            }
            return iEntity.GetRota();
        }

        public Vector3 GetForward() {
            if (isClear) {
                return Vector3.forward;
            }
            return iEntity.GetForward();
        }

        /// <summary>
        /// 是否可以成为目标：true 可以，false 不可以
        /// </summary>
        /// <returns></returns>
        public bool IsGodMode() {
            return isGodMode || isDead || isOnline == false;
        }
        
        public bool IsDead() {
            return isDead;
        }
        public bool IsLocalGPO() {
            return false;
        }
        
        private void OnSetIsOnlineCallBack(ISystemMsg body, SE_Network.Event_SetIsOnline ent) {
            isOnline = ent.IsOnline;
        }

        private void OnGetIsOnlineCallBack(ISystemMsg body, SE_Network.Event_GetIsOnline ent) {
            ent.CallBack(isOnline);
        }

        private void OnIsGodModeCallBack(ISystemMsg body, SE_GPO.Event_IsGodMode ent) {
            isGodMode = ent.IsTrue;
        }
        
        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            isDead = ent.IsDead;
        }
        
        public GPOData.GPOType GetGPOType() {
            return gpoType;
        }
        
        public List<IHitType> GetAllCanHitPart() {
            if (gpoEntity == null) {
                return new List<IHitType>();
            }
            return gpoEntity.GetAllCanHitPart();
        }

        private void OnSetRemoveGPOCallBack(ISystemMsg body, SE_GPO.Event_StartRemoveGPO ent) {
            Rpc(new Proto_GPO.Rpc_RemoveGPO());
        }

        public Transform GetBodyTran(GPOData.PartEnum gpoType) {
            if (isClear) {
                return null;
            }
            return iEntity.GetBodyTran(gpoType);
        }

        public List<Transform> GetBodyTranList(GPOData.PartEnum gpoType) {
            if (isClear) {
                return null;
            }
            return iEntity.GetBodyTranList(gpoType);
        }
        
        public Transform GetTargetTransform() {
            Transform lockTarget = null;
            lockTarget = GetBodyTran(GPOData.PartEnum.Head);
            if (lockTarget == null) {
                lockTarget = GetBodyTran(GPOData.PartEnum.Body);
            }
            if (lockTarget == null) {
                lockTarget = GetBodyTran(GPOData.PartEnum.RootBody);
            }
            return lockTarget;
        }
    }
}