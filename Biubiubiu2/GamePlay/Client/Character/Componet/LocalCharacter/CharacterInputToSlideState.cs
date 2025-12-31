using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterInputToSlideState : ClientCharacterComponent {
        public float slideSpeed = 15f; // 初始滑铲速度
        public float slideDuration = 1.2f; // 滑铲持续时间（秒）
        public float slideHeight = 0.5f; // 滑铲时角色的高度
        private bool isSliding = false; // 滑铲状态
        private float slideTimer; // 滑铲计时器
        private Vector3 slideDirection; // 滑铲方向
        private CharacterController characterController;
        private float originalHeight; // 角色原始高度
        private Vector3 originalCenter; // 角色原始碰撞中心
        private Vector3 slideCenter; // 角色滑铲碰撞中心
        private bool isGround = true;
        private Transform BodyTran;

        protected override void OnAwake() {
            MsgRegister.Register<CM_InputPlayer.Dodge>(OnDodgaCallBack);
            MsgRegister.Register<CM_InputPlayer.Jump>(OnJumpCallBack);
            this.mySystem.Register<CE_Character.IsGround>(OnIsGroundCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Character.Rpc_Slide.ID, OnRpcSlideCallBack);
            AddProtoCallBack(Proto_Character.TargetRpc_Slide.ID, OnTargetRpcSlideCallBack);
        }

        private void OnRpcSlideCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Rpc_Slide)cmdData;
        }

        private void OnTargetRpcSlideCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.TargetRpc_Slide)cmdData;
        }

        protected override void OnSetEntityObj(IEntity entity) {
            base.OnSetEntityObj(entity);
            var cEntity = (CharacterEntity)iEntity;
            this.characterController = cEntity.characterController;
            originalHeight = characterController.height;
            originalCenter = characterController.center;
            slideCenter = originalCenter - new Vector3(0, originalHeight - slideHeight, 0) / 2;
            BodyTran = cEntity.cBodyTran;
        }

        protected override void OnClear() {
            base.OnClear();
            characterController = null;
            MsgRegister.Unregister<CM_InputPlayer.Dodge>(OnDodgaCallBack);
            MsgRegister.Unregister<CM_InputPlayer.Jump>(OnJumpCallBack);
            this.mySystem.Unregister<CE_Character.IsGround>(OnIsGroundCallBack);
            RemoveProtoCallBack(Proto_Character.Rpc_Slide.ID, OnRpcSlideCallBack);
            RemoveProtoCallBack(Proto_Character.TargetRpc_Slide.ID, OnTargetRpcSlideCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            if (isSliding) {
                Slide();
            }
        }

        private void OnIsGroundCallBack(ISystemMsg body, CE_Character.IsGround envData) {
            isGround = envData.IsTrue;
        }

        public void OnDodgaCallBack(CM_InputPlayer.Dodge ent) {
            if (!isSliding) {
                StartSlide();
            }
        }

        private void StartSlide() {
            SetIsSliding(true);
            slideTimer = slideDuration;
            slideDirection = characterController.velocity.normalized; // 记录滑铲方向
            if (slideDirection == Vector3.zero) {
                slideDirection = BodyTran.forward;
            }
            mySystem.Dispatcher(new CE_Character.Event_StartSlideMove {
            });
            characterController.height = slideHeight; // 降低角色高度以实现滑铲状态
            characterController.center = slideCenter;
        }

        private void Slide() {
            // 计算当前滑铲速度，逐渐减速
            var currentSlideSpeed = Mathf.Lerp(0, slideSpeed, slideTimer / slideDuration);
            // 移动角色
            var slideVelocity = BodyTran.forward * currentSlideSpeed * Time.deltaTime;
            // 更新滑铲计时器
            slideTimer -= Time.deltaTime;
            // 滑铲结束条件
            if (slideTimer <= 0.1f) {
                EndSlide();
            } else {
                mySystem.Dispatcher(new CE_Character.Event_SlideMove {
                    SlideVelocity = slideVelocity,
                    IsSlide = true,
                });
            }
        }

        void EndSlide() {
            if (isSliding == false) {
                return;
            }
            SetIsSliding(false);
            characterController.height = originalHeight; // 恢复角色原始高度
            characterController.center = originalCenter;
            mySystem.Dispatcher(new CE_Character.Event_SlideMove {
                SlideVelocity = Vector3.zero,
                IsSlide = false,
            });
        }

        private void SetIsSliding(bool isSliding) {
            this.isSliding = isSliding;
            Cmd(new Proto_Character.Cmd_Slide {
                isSlide = isSliding
            });
        }

        public void OnJumpCallBack(CM_InputPlayer.Jump ent) {
            CancelDodga();
        }

        private void CancelDodga() {
            EndSlide();
        }
        
    }
}