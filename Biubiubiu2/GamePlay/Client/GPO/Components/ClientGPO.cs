using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPO : ComponentBase, IGPO {
        public struct InitData : SystemBase.IComponentInitData {
            public GPOData.GPOType GpoType;
            public int TeamId;
            public int GpoId;
            public bool IsLocalGPO;
            public IGPOM GpoMData;
        }
        private GPOData.GPOType gpoType;
        private GPOData.AttributeData gpoAttribute;
        private IGPOM gpoMData = null;
        private int gpoId = 0;
        private int teamId = 0;
        private bool isLocalGPO = false;
        private bool isGodMode = false;
        private bool isDead = false;
        private bool isInit = false;
        private bool isOnline = true;
        private C_GPO_Base gpoSystem;
        private EntityBase gpoEntity;
        
        protected override void OnAwake() {
            base.OnAwake();
            gpoSystem = (C_GPO_Base)mySystem;
            mySystem.Register<CE_GPO.Event_IsGodMode>(OnIsGodModeCallBack);
            mySystem.Register<CE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Register<CE_Network.IsOnline>(OnIsOnlineCallBack);
            var initData = (InitData)initDataBase;
            gpoMData = initData.GpoMData;
            this.gpoType = initData.GpoType;
            this.teamId = initData.TeamId;
            this.gpoId = initData.GpoId;
            this.isLocalGPO = initData.IsLocalGPO;
            SetGPO();
        }
        
        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Dispatcher(new CM_GPO.AddGPO {
                IGpo = this,
            });
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_GPO.TargetRpc_SyncGPOID.ID, TargetRpcSyncGPOIDSpawnDataCallBack);
            RemoveProtoCallBack(Proto_GPO.Rpc_RemoveGPO.ID, OnRemoveGPOCallBack);
            mySystem.Unregister<CE_GPO.Event_IsGodMode>(OnIsGodModeCallBack);
            mySystem.Unregister<CE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Unregister<CE_Network.IsOnline>(OnIsOnlineCallBack);
            MsgRegister.Dispatcher(new CM_GPO.RemoveGPO {
                GpoId = gpoId
            });
            gpoSystem = null;
            gpoMData = null;
            gpoEntity = null;
        }
        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            gpoEntity = (EntityBase)iEntity;
        }
        
        public IGPOM GetMData() {
            return this.gpoMData;
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
          
        public void SetGPO() {
            if (isInit || this.gpoId <= 0) {
                return;
            }
            this.isInit = true;
            SetSign(this.gpoMData.GetSign());
            this.iEntity.SetGpo(this);
            this.mySystem.SetIGPO(this);
        }
        
        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_GPO.Rpc_RemoveGPO.ID, OnRemoveGPOCallBack);
            AddProtoCallBack(Proto_GPO.TargetRpc_SyncGPOID.ID, TargetRpcSyncGPOIDSpawnDataCallBack);
        }

        private void TargetRpcSyncGPOIDSpawnDataCallBack(INetwork network, IProto_Doc data) {
            var rpcData = (Proto_GPO.TargetRpc_SyncGPOID)data;
            this.gpoType = rpcData.gpoType;
            this.teamId = rpcData.teamID;
            this.gpoId = rpcData.gpoID;
            SetGPO();
        }

        private void OnRemoveGPOCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            mySystem.Dispatcher(new CE_GPO.Event_StartRemoveGPO());
        }

        private void OnIsGodModeCallBack(ISystemMsg body, CE_GPO.Event_IsGodMode ent) {
            isGodMode = ent.IsTrue;
        }
        
        private void OnIsOnlineCallBack(ISystemMsg body, CE_Network.IsOnline ent) {
            isOnline = ent.IsTrue;
        }

        private void OnSetIsDeadCallBack(ISystemMsg body, CE_GPO.Event_SetIsDead ent) {
            isDead = ent.IsDead;
        }
        
        public int GetTeamID() {
            return teamId;
        }

        public bool IsGodMode() {
            return isGodMode || isDead || isOnline == false;
        }

        public int GetGpoID() {
            return gpoId;
        }

        public Vector3 GetPoint() {
            return iEntity.GetPoint();
        }

        public Quaternion GetRota() {
            return iEntity.GetRota();
        }        
        
        public Vector3 GetForward() {
            return iEntity.GetForward();
        }

        public GPOData.GPOType GetGPOType() {
            return gpoType;
        }

        public bool IsLocalGPO() {
            return isLocalGPO;
        }

        public bool IsDead() {
            return isDead;
        }
        
        public List<IHitType> GetAllCanHitPart() {
            if (gpoEntity == null) {
                return new List<IHitType>();
            }
            return gpoEntity.GetAllCanHitPart();
        }

        public Transform GetBodyTran(GPOData.PartEnum gpoType) {
            if (iEntity == null) {
                return null;
            }
            return iEntity.GetBodyTran(gpoType);
        }

        public List<Transform> GetBodyTranList(GPOData.PartEnum gpoType) {
            if (iEntity == null) {
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