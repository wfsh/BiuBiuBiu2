using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    //怪物 GameOBJ 显示隐藏
    public class ServerGPOShowEntity : ServerNetworkComponentBase {
        private bool isShowEnity = true;
        private bool isPlayerDrive = false;
        private bool isDeadShowEntity = true;
        private bool isShowEnityForAnim = true;
        private bool deadAutoHideEntity = true;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Register<SE_GPO.Event_PlayerDrive>(OnPlayerDriveCallBack);
            mySystem.Register<SE_Entity.Event_SetDeadAutoHideEntity>(OnSetDeadAutoHideEntityCallBack);
            mySystem.Register<SE_Entity.Event_SetShowEntityForAnim>(OnShowEntityForAnimCallBack);
            mySystem.Register<SE_Entity.Event_GetShowEntity>(OnGetShowEntityCallBack);
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
            mySystem.Unregister<SE_Entity.Event_SetDeadAutoHideEntity>(OnSetDeadAutoHideEntityCallBack);
            mySystem.Unregister<SE_Entity.Event_SetShowEntityForAnim>(OnShowEntityForAnimCallBack);
            mySystem.Unregister<SE_Entity.Event_GetShowEntity>(OnGetShowEntityCallBack);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            ShowEntity(isShowEnity);
        }

        protected override ITargetRpc SyncData() {
            return new Proto_GPO.TargetRpc_IsShowEntity {
                isShowEntity = isShowEnity,
            };
        }

        private void OnUpdate(float deltaTime) {
            UpdateShowEntityState();
        }

        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            var isDead = ent.IsDead;
            if (isDead && deadAutoHideEntity) {
                isDeadShowEntity = false;
            } else {
                isDeadShowEntity = true;
            }
        }
        
        private void OnSetDeadAutoHideEntityCallBack(ISystemMsg body, SE_Entity.Event_SetDeadAutoHideEntity ent) {
            deadAutoHideEntity = ent.isDeadAutoHideEntity;
        }

        private void OnShowEntityForAnimCallBack(ISystemMsg body, SE_Entity.Event_SetShowEntityForAnim ent) {
            isShowEnityForAnim = ent.IsShow;
        }

        private void OnGetShowEntityCallBack(ISystemMsg body, SE_Entity.Event_GetShowEntity ent) {
            ent.CallBack?.Invoke(isShowEnityForAnim);
        }


        private void OnPlayerDriveCallBack(ISystemMsg body, SE_GPO.Event_PlayerDrive ent) {
            isPlayerDrive = ent.IsDrive;
        }

        private void ShowEntity(bool isShow) {
            if (iEntity is EntityBase) {
                var entity = (EntityBase)iEntity;
                entity.SetActive(isShow);
            }
        }

        private void UpdateShowEntityState() {
            var isShow = true;
            if (isPlayerDrive || isDeadShowEntity == false || isShowEnityForAnim == false) {
                isShow = false;
            }
            if (isShowEnity != isShow) {
                isShowEnity = isShow;
                mySystem.Dispatcher(new SE_Entity.Event_IsShowEntity {
                    IsShow = isShowEnity,
                });
                Rpc(new Proto_GPO.Rpc_IsShowEntity {
                    isTrue = isShowEnity,
                });
                ShowEntity(isShowEnity);
            }
        }
    }
}