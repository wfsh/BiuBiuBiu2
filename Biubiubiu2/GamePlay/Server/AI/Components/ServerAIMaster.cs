using System;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIMaster : ServerNetworkComponentBase {
        private S_AI_Base aiSystem;
        private float masterGPODistance = 0f;
        private float checkDistanceTime = 0.0f;

        protected override void OnAwake() {
            Register<SE_AI.Event_TakeBack>(OnTakeBackCallBack);
            Register<SE_AI.Event_GetMasterGPO>(OnGetMasterGPOCallBack);
            Register<SE_AI.Event_MasterGPODistance>(OnMasterGPODistanceCallBack);
            Register<SE_GPO.Event_SetHurtTOGpo>(OnSetHurtTOGpoCallBack);
            aiSystem = (S_AI_Base)mySystem;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            aiSystem = null;
            RemoveUpdate(OnUpdate);
            Unregister<SE_AI.Event_TakeBack>(OnTakeBackCallBack);
            Unregister<SE_AI.Event_GetMasterGPO>(OnGetMasterGPOCallBack);
            Unregister<SE_AI.Event_MasterGPODistance>(OnMasterGPODistanceCallBack);
            Unregister<SE_GPO.Event_SetHurtTOGpo>(OnSetHurtTOGpoCallBack);
            base.OnClear();
        }

        protected override ITargetRpc SyncData() {
            if (aiSystem.MasterGPO == null) {
                return new Proto_AI.TargetRpc_AIMaster() {
                    masterGpoID = 0
                };
            } else {
                return new Proto_AI.TargetRpc_AIMaster() {
                    masterGpoID = aiSystem.MasterGPO.GetGpoID()
                };
            }
        }

        private void OnUpdate(float deltaTime) {
            var masterGPO = aiSystem.MasterGPO;
            if (masterGPO == null) {
                return;
            }
            if (checkDistanceTime > 0f) {
                checkDistanceTime -= Time.deltaTime;
                return;
            }
            checkDistanceTime = 0.5f;
            masterGPODistance = Vector3.Distance(iEntity.GetPoint(), masterGPO.GetPoint());
        }

        private void OnGetMasterGPOCallBack(ISystemMsg body, SE_AI.Event_GetMasterGPO ent) {
            ent.CallBack(aiSystem.MasterGPO);
        }

        private void OnTakeBackCallBack(ISystemMsg body, SE_AI.Event_TakeBack ent) {
            Rpc(new Proto_AI.Rpc_TakeBack());
            MsgRegister.Dispatcher(new SM_AI.Event_RemoveAI {
                GpoId = iGPO.GetGpoID()
            });
        }

        private void OnMasterGPODistanceCallBack(ISystemMsg body, SE_AI.Event_MasterGPODistance ent) {
            ent.CallBack(masterGPODistance);
        }
        
        private void OnSetHurtTOGpoCallBack(ISystemMsg body, SE_GPO.Event_SetHurtTOGpo ent) {
            var masterGPO = aiSystem.MasterGPO;
            if (masterGPO == null) {
                return;
            }
            masterGPO.Dispatcher(new SE_GPO.Event_SetHurtTOGpo {
                HurtValue = ent.HurtValue,
                AttackGPO = masterGPO,
                HurtGPO = ent.HurtGPO,
                DamageType = ent.DamageType,
                AttackItemId = ent.AttackItemId
            });
        }
    }
}