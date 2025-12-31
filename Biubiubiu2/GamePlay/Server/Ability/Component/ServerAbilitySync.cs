using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilitySync : ComponentBase {
        private List<INetworkCharacter> spawnNetworks = new List<INetworkCharacter>();
        // 同步距离
        private float SyncDistance = 300;
        private int abilityId = 0;
        private int fireGpoId = 0;
        private bool isSend = false;

        protected override void OnAwake() {
            this.mySystem.Register<SE_Ability.RPCAbility>(OnRPCAbilityCallBack);
            var system = (S_Ability_Base)mySystem;
            abilityId = system.AbilityId;
            fireGpoId = system.FireGPO == null ? 0 : system.FireGPO.GetGpoID();
        }

        protected override void OnClear() {
            this.mySystem.Unregister<SE_Ability.RPCAbility>(OnRPCAbilityCallBack);
            // RpcRemoveAbility();  // 短效 的 AB 走客户端自己生命周期清除，暂时屏蔽
            spawnNetworks.Clear();
        }

        private void OnRPCAbilityCallBack(ISystemMsg body, SE_Ability.RPCAbility ent) {
            if (isSend) {
                Debug.LogError("短效的 AB，一个 system 只能调用一次 RPCAbility，如果需要播放多个特效，需要单独拆分 AB ，或使用 SAB_PlayEffect");
                return;
            }
            isSend = true;
            MsgRegister.Dispatcher(new M_Network.RPCSerialize {
                ProtoDoc = ent.ProtoData,
                ConnID = ConnID,
                Channel = ent.ProtoData.GetChannel(),
                CallBack = (channel, connId, bytes) => {
                    RpcForNetworkList(fireGpoId, bytes);
                },
            });
        }

        private void RpcRemoveAbility() {
            if (this.ConnID > 0) {
                return;
            }
            this.networkBase.TargetRpcList(spawnNetworks, new Proto_Ability.TargetRpc_RemoveAbility {
                abilityId = abilityId,
            });
        }

        private void RpcForNetworkList(int fireGpoId, byte[] proto) {
            MsgRegister.Dispatcher(new M_Network.GetAllNetworkForPoint {
                CallBack = list => {
                    spawnNetworks = list;
                },
                Distance = SyncDistance,
                Point = iEntity.GetPoint(),
            });
            this.networkBase.TargetRpcList(spawnNetworks, new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = (ushort)fireGpoId, 
                abilityId = abilityId, 
                protoDoc = proto,
            });
        }
    }
}