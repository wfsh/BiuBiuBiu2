using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientNetworkAbilitySync : ComponentBase {
        private INetwork network;

        protected override void OnAwake() {
            MsgRegister.Register<CM_Network.SpawnWorldNetwork>(OnSpawnWorldNetworkCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Ability.TargetRpc_RemoveAbility.ID, RpcRemoveAbilityCallBack);
            RemoveProtoCallBack(Proto_Ability.TargetRpc_PlayAbility.ID, RpcPlayAbilityCallBack);
            MsgRegister.Unregister<CM_Network.SpawnWorldNetwork>(OnSpawnWorldNetworkCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Ability.TargetRpc_RemoveAbility.ID, RpcRemoveAbilityCallBack);
            AddProtoCallBack(Proto_Ability.TargetRpc_PlayAbility.ID, RpcPlayAbilityCallBack);
        }

        private void RpcRemoveAbilityCallBack(INetwork network, IProto_Doc data) {
            var rpcData = (Proto_Ability.TargetRpc_RemoveAbility)data;
            this.mySystem.Dispatcher(new CE_Ability.RemoveAbility {
                AbilityId = rpcData.abilityId,
            });
        }

        private void RpcPlayAbilityCallBack(INetwork network, IProto_Doc data) {
            var rpcData = (Proto_Ability.TargetRpc_PlayAbility)data;
            PlayAbility(rpcData, -1);
        }
        
        

        private void PlayAbility(Proto_Ability.TargetRpc_PlayAbility rpcData, int connId) {
            MsgRegister.Dispatcher(new M_Network.RPCUnSerialize {
                Datas = rpcData.protoDoc,
                ConnID = connId,
                CallBack = (cid, protoDoc) => {
                    this.mySystem.Dispatcher(new CE_Ability.PlayAbility {
                        sign = protoDoc.GetID(), 
                        fireGpoId = rpcData.fireGpoId,
                        abilityId = rpcData.abilityId,
                        abilityData = protoDoc, 
                        connId = cid,
                    });
                },
            });
        }

        private void OnSpawnWorldNetworkCallBack(CM_Network.SpawnWorldNetwork ent) {
            if (ent.ConnType != NetworkData.SpawnConnType.Ability) {
                return;
            }
            if (ent.ProtoDoc.GetID() == Proto_Ability.TargetRpc_PlayAbility.ID) {
                var rpcData = (Proto_Ability.TargetRpc_PlayAbility)ent.ProtoDoc;
                PlayAbility(rpcData, ent.ConnID);
            } else {
                Debug.LogError("没有注册 World Spawn 事件:" + ent.ProtoDoc.GetID());
            }
        }
    }
}