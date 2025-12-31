using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterSlide : ServerCharacterComponent {
        private bool isSliding = false;

        protected override void OnAwake() {
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            isSliding = false;
            RemoveProtoCallBack(Proto_Character.Cmd_Slide.ID, OnSlideCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Character.Cmd_Slide.ID, OnSlideCallBack);
        }

        protected override ITargetRpc SyncData() {
            return new Proto_Character.TargetRpc_Slide {
                isSlide = isSliding
            };
        }

        private void OnSlideCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Cmd_Slide)cmdData;
            mySystem.Dispatcher(new SE_GPO.Event_Slide {
                GPOId = GpoID,
                IsSlide = data.isSlide
            });
            isSliding = data.isSlide;
            Rpc(new Proto_Character.Rpc_Slide {
                isSlide = isSliding,
            });
        }
    }
}