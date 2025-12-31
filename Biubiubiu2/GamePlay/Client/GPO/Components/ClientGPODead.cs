using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPODead : ComponentBase {
        private bool isDead = false;
        protected override void OnClear() {
            RemoveProtoCallBack(Proto_GPO.Rpc_Dead.ID, OnDeadCallBack);
            RemoveProtoCallBack(Proto_GPO.Rpc_ReLife.ID, OnReLifeCallBack);
            base.OnClear();
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_GPO.Rpc_Dead.ID, OnDeadCallBack);
            AddProtoCallBack(Proto_GPO.Rpc_ReLife.ID, OnReLifeCallBack);
        }

        private void OnDeadCallBack(INetwork network, IProto_Doc docData) {
            if (iGPO.IsLocalGPO()) {
                AudioPoolManager.OnPlayAudio(AssetURL.GetAudio1P("Role_Dead"), iEntity.GetPoint());
            } else {
                AudioPoolManager.OnPlayAudio(AssetURL.GetAudio3P("Role_Dead_3P"), iEntity.GetPoint());
            }
            mySystem.Dispatcher(new CE_GPO.Event_SetIsDead {
                IsDead = true,
                DeadGPO = iGPO
            });
            OnDead();
        }

        virtual protected void OnDead() {
        }

        private void OnReLifeCallBack(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_GPO.Rpc_ReLife)docData;
            mySystem.Dispatcher(new CE_GPO.Event_SetIsDead {
                IsDead = false,
                DeadGPO = iGPO
            });
            iEntity.SetPoint(rpcData.relifePos);
            OnReLife();
        }

        virtual protected void OnReLife() {
        }
    }
}