using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterOtherSlideState : ClientCharacterComponent {
        private bool isSliding = false; // 滑铲状态

        protected override void OnAwake() {
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Character.Rpc_Slide.ID, OnRpcSlideCallBack);
            RemoveProtoCallBack(Proto_Character.TargetRpc_Slide.ID, OnTargetRpcSlideCallBack);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Character.Rpc_Slide.ID, OnRpcSlideCallBack);
            AddProtoCallBack(Proto_Character.TargetRpc_Slide.ID, OnTargetRpcSlideCallBack);
        }

        private void OnRpcSlideCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Rpc_Slide)cmdData;
            SetIsSliding(data.isSlide);
        }

        private void OnTargetRpcSlideCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.TargetRpc_Slide)cmdData;
            SetIsSliding(data.isSlide);
        }

        private void SetIsSliding(bool isSliding) {
            this.isSliding = isSliding;
            mySystem.Dispatcher(new CE_Character.Event_SlideMove {
                SlideVelocity = Vector3.zero,
                IsSlide = isSliding,
            });
        }
    }
}