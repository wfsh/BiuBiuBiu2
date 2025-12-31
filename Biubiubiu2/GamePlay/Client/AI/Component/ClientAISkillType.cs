using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAISkillType : ComponentBase {
        private bool isSkillType = false;
        private EntityBase entityBase;
        protected override void OnAwake() {
            mySystem.Register<CE_AI.Event_GetSkillType>(OnGetSkillTypeCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            RemoveProtoCallBack(Proto_AI.Rpc_SkillType.ID, OnRpcSkillType);
            RemoveProtoCallBack(Proto_AI.TargetRpc_SkillType.ID, OnTargetRpcSkillType);
            mySystem.Unregister<CE_AI.Event_GetSkillType>(OnGetSkillTypeCallBack);
            base.OnClear();
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.Rpc_SkillType.ID, OnRpcSkillType);
            AddProtoCallBack(Proto_AI.TargetRpc_SkillType.ID, OnTargetRpcSkillType);
        }

        protected override void OnSetEntityObj(IEntity entity) {
            base.OnSetEntityObj(entity);
            entityBase = (EntityBase)entity;
            CheckSkillType();
        }
        private void OnTargetRpcSkillType(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_AI.TargetRpc_SkillType)docData;
            isSkillType = rpcData.isSkillType;
            mySystem.Dispatcher(new CE_AI.Event_IsSkillType {
                IsSkillType = isSkillType
            });
            CheckSkillType();
        }

        private void OnRpcSkillType(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_AI.Rpc_SkillType)docData;
            isSkillType = rpcData.isSkillType;
            mySystem.Dispatcher(new CE_AI.Event_IsSkillType {
                IsSkillType = isSkillType
            });
            CheckSkillType();
        }
        private void OnGetSkillTypeCallBack(ISystemMsg body, CE_AI.Event_GetSkillType ent) {
            ent.CallBack.Invoke(isSkillType);
        }
        
        private void CheckSkillType() {
            if (entityBase == null) {
                return;
            }
            entityBase.SetActive(isSkillType == false);
        }
    }
}