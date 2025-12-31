using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterMove : ServerCharacterComponent {
        protected override void OnAwake() {
            mySystem.Register<SE_GPO.Event_KnockbackGPO>(OnKnockbackGPOCallBack);
            mySystem.Register<SE_GPO.Event_StrikeFlyGPO>(OnStrikeFlyGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_KnockbackGPO>(OnKnockbackGPOCallBack);
            mySystem.Unregister<SE_GPO.Event_StrikeFlyGPO>(OnStrikeFlyGPOCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_MoveDir.ID, OnMoveDirCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Character.Cmd_MoveDir.ID, OnMoveDirCallBack);
        }

        private void OnMoveDirCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Cmd_MoveDir)cmdData;
            Rpc(new Proto_Character.Rpc_MoveDir {
                moveX = data.moveX, moveY = data.moveY,
            });
        }

        public void OnStrikeFlyGPOCallBack(ISystemMsg body, SE_GPO.Event_StrikeFlyGPO ent) {
            ent.Dir.Normalize();
            TargetRpc(networkBase, new Proto_Character.TargetRpc_StrikeFly {
                Dir = ent.Dir, Force = ent.Force, Duration = ent.Duration
            });
        }

        public void OnKnockbackGPOCallBack(ISystemMsg body, SE_GPO.Event_KnockbackGPO ent) {
            TargetRpc(networkBase, new Proto_Character.TargetRpc_Knockback() {
                Dir = ent.Dir, Speed = ent.Speed, Duration = ent.Duration
            });
        }
    }
}