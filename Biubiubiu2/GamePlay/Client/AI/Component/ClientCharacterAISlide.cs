using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterAISlide : ComponentBase {
        private GameObject effectObj;
        private Transform BodyTran;
        private bool isSliding = false; // 滑铲状态
        
        protected override void OnAwake() {
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.TargetRpc_Anim.ID, OnPlayAnim);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_AI.TargetRpc_Anim.ID, OnPlayAnim);
            BodyTran = null;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var cEntity = (EntityBase)iEntity;
            BodyTran = cEntity.transform;
        }

        private void OnPlayAnim(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_AI.TargetRpc_Anim)docData;
            if (rpcData.animId == AnimConfig_HTR.Anim_soccer_Tackle) {
                isSliding = true;
            } else {
                isSliding = false;
            }
        }
    }
}