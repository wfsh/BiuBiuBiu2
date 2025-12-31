using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOIsGodMode : ServerNetworkComponentBase {
        public bool isGodMode = false;
        public bool isDead = false;
        public bool isDrive = false;
        private float protectTime = 0.0f;
        public bool isShowEntity = true;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Register<SE_GPO.Event_PlayerDrive>(OnPlayerDriveCallBack);
            mySystem.Register<SE_GPO.Event_BecomeTargetProtectTime>(OnBecomeTargetProtectTimeCallBack);
            mySystem.Register<SE_Entity.Event_IsShowEntity>(OnIsShowEntityCallBack);
        }
   
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Unregister<SE_GPO.Event_PlayerDrive>(OnPlayerDriveCallBack);
            mySystem.Unregister<SE_GPO.Event_BecomeTargetProtectTime>(OnBecomeTargetProtectTimeCallBack);
            mySystem.Unregister<SE_Entity.Event_IsShowEntity>(OnIsShowEntityCallBack);
        }

        protected override ITargetRpc SyncData() {
            return new Proto_GPO.TargetRpc_IsGodMode {
                isTrue = isGodMode,
            };
        }

        private void OnUpdate(float deltaTime) {
            if (protectTime > 0) {
                protectTime -= deltaTime;
            }
            UpdateBecomeTargetState();
        }

        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            isDead = ent.IsDead;
        }

        private void OnIsShowEntityCallBack(ISystemMsg body, SE_Entity.Event_IsShowEntity ent) {
            isShowEntity = ent.IsShow;
        }
        
        private void OnPlayerDriveCallBack(ISystemMsg body, SE_GPO.Event_PlayerDrive ent) {
            isDrive = ent.IsDrive;
            if (isDrive == false) {
                protectTime = Mathf.Max(1f, protectTime);
            }
        }

        private void OnBecomeTargetProtectTimeCallBack(ISystemMsg body, SE_GPO.Event_BecomeTargetProtectTime ent) {
            protectTime = ent.ProtectTime;
        }

        private void UpdateBecomeTargetState() {
            var isTrue = false;
            if (isSetEntityObj == false || isShowEntity == false || isDrive || isDead || protectTime > 0f) {
                isTrue = true;
            }
            if (isGodMode != isTrue) {
                isGodMode = isTrue;
                mySystem.Dispatcher(new SE_GPO.Event_IsGodMode {
                    IsTrue = isTrue,
                });
                Rpc(new Proto_GPO.Rpc_IsGodMode {
                    isTrue = isTrue
                });
            }
        }
    }
}