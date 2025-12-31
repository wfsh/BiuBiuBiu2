using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIMaster : ComponentBase {
        private int masterId = 0;
        private IGPO masterGpo = null;
        private C_AI_Base aiSystem;

        protected override void OnAwake() {
            aiSystem = (C_AI_Base)mySystem;
            MsgRegister.Register<CM_GPO.AddGPO>(OnAddGPOCallBack);
            MsgRegister.Register<CM_GPO.RemoveGPO>(OnRemoveGPOCallBack);
            mySystem.Register<CE_AI.Event_GetMasterGpoID>(OnGetMasterGPOIdCallBack);
            mySystem.Register<CE_AI.Event_GetMaterGPO>(OnGetMaterGPOCallBack);
        }

        protected override void OnClear() {
            MsgRegister.Unregister<CM_GPO.AddGPO>(OnAddGPOCallBack);
            MsgRegister.Unregister<CM_GPO.RemoveGPO>(OnRemoveGPOCallBack);
            mySystem.Unregister<CE_AI.Event_GetMasterGpoID>(OnGetMasterGPOIdCallBack);
            mySystem.Unregister<CE_AI.Event_GetMaterGPO>(OnGetMaterGPOCallBack);
            RemoveProtoCallBack(Proto_AI.TargetRpc_AIMaster.ID, OnMonsterMaster);
            RemoveProtoCallBack(Proto_AI.Rpc_TakeBack.ID, OnTakeBackMaster);
            aiSystem = null;
            base.OnClear();
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.TargetRpc_AIMaster.ID, OnMonsterMaster);
            AddProtoCallBack(Proto_AI.Rpc_TakeBack.ID, OnTakeBackMaster);
        }

        private void OnAddGPOCallBack(CM_GPO.AddGPO ent) {
            if (ent.IGpo == null || ent.IGpo.GetGpoID() != masterId) {
                return;
            }
            SetMasterGPO(ent.IGpo);
        }

        private void SetMasterGPO(IGPO gpo) {
            if (gpo == null || gpo.GetGpoID() != masterId) {
                return;
            }
            masterGpo = gpo;
            aiSystem.SetMasterGPO(gpo);
            gpo.Dispatcher(new CE_AI.Event_SetSummonAI {
                SummonAIGPO = iGPO
            });
        }

        private void OnRemoveGPOCallBack(CM_GPO.RemoveGPO ent) {
            if (ent.GpoId != masterId) {
                return;
            }
            aiSystem.SetMasterGPO(null);
        }

        private void OnMonsterMaster(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_AI.TargetRpc_AIMaster)docData;
            masterId = rpcData.masterGpoID;
            if (masterId != 0) {
                MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                    GpoId = masterId,
                    CallBack = SetMasterGPO
                });
            }
        }

        private void OnTakeBackMaster(INetwork network, IProto_Doc docData) {
            Dispatcher(new CE_AI.RemoveAI {
                GpoId = GpoID
            });
        }
        
        private void OnGetMasterGPOIdCallBack(ISystemMsg body, CE_AI.Event_GetMasterGpoID ent) {
            ent.CallBack(masterId);
        }
        
        private void OnGetMaterGPOCallBack(ISystemMsg body, CE_AI.Event_GetMaterGPO ent) {
            ent.CallBack(masterGpo);
        }
    }
}