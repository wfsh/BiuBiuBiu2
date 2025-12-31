using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterAISlide : ServerNetworkComponentBase {
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
            this.mySystem.Register<SE_Behaviour.Event_OnSlide>(OnSlideCallBack);
            this.mySystem.Register<SE_GPO.Event_IsGround>(OnIsGroundCallBack);
            this.mySystem.Register<SE_GPO.Event_JumpTypeChange>(OnJumpTypeChangeCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity entity) {
            base.OnSetEntityObj(entity);
            var cEntity = (EntityBase)iEntity;
            this.characterController = cEntity.GetComponent<CharacterController>();
            originalHeight = characterController.height;
            originalCenter = characterController.center;
            slideCenter = originalCenter - new Vector3(0, originalHeight - slideHeight, 0) / 2;
            BodyTran = cEntity.transform;
        }

        protected override void OnClear() {
            base.OnClear();
            characterController = null;
            this.mySystem.Unregister<SE_GPO.Event_IsGround>(OnIsGroundCallBack);
            this.mySystem.Unregister<SE_GPO.Event_JumpTypeChange>(OnJumpTypeChangeCallBack);
            this.mySystem.Unregister<SE_Behaviour.Event_OnSlide>(OnSlideCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            if (isSliding) {
                Slide();
            }
        }
        private void OnIsGroundCallBack(ISystemMsg body, SE_GPO.Event_IsGround envData) {
            isGround = envData.IsTrue;
        }

        public void OnSlideCallBack(ISystemMsg body, SE_Behaviour.Event_OnSlide ent) {
            StartSlide();
        }

        private void StartSlide() {
            if (isGround == false || isSliding) {
                return;
            }
            isSliding = true;
            slideTimer = slideDuration;
            slideDirection = characterController.velocity.normalized; // 记录滑铲方向
            if (slideDirection == Vector3.zero) {
                slideDirection = BodyTran.forward;
            }
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
                mySystem.Dispatcher(new SE_GPO.Event_SlideMove {
                    SlideVelocity = slideVelocity, IsSlide = true,
                });
            }
        }

        void EndSlide() {
            if (isSliding == false) {
                return;
            }
            isSliding = false;
            characterController.height = originalHeight; // 恢复角色原始高度
            characterController.center = originalCenter;
            mySystem.Dispatcher(new SE_GPO.Event_SlideMove {
                SlideVelocity = Vector3.zero, IsSlide = false,
            });
        }

        public void OnJumpTypeChangeCallBack(ISystemMsg body, SE_GPO.Event_JumpTypeChange ent) {
            if (ent.JumpType == CharacterData.JumpType.None) {
                return;
            }
            CancelDodga();
        }

        private void CancelDodga() {
            EndSlide();
        }
    }
}