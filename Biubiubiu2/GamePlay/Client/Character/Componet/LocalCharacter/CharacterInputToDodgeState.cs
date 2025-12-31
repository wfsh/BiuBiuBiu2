using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterInputToDodgeState : ClientCharacterComponent {
        private float dodgaLimitTime = 0.0f;
        private float dodgaTime = 0.0f;
        private bool isDodga = false;
        private bool isGround = false;
        private bool isStartDodgaDownTime = false;

        protected override void OnAwake() {
            // MsgRegister.Register<CM_InputPlayer.Dodge>(OnDodgaCallBack);
            MsgRegister.Register<CM_InputPlayer.Jump>(OnJumpCallBack);
            this.mySystem.Register<CE_Character.IsGround>(OnIsGroundCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            // MsgRegister.Unregister<CM_InputPlayer.Dodge>(OnDodgaCallBack);
            MsgRegister.Unregister<CM_InputPlayer.Jump>(OnJumpCallBack);
            this.mySystem.Unregister<CE_Character.IsGround>(OnIsGroundCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            CountDodgaLimitTime(deltaTime);
            CountDodgeTime(deltaTime);
        }

        private void OnIsGroundCallBack(ISystemMsg body, CE_Character.IsGround envData) {
            isGround = envData.IsTrue;
            if (isGround) {
                CancelDodga();
            }
        }

        private void CountDodgaLimitTime(float deltaTime) {
            if (isStartDodgaDownTime == false) {
                if (isGround == false || isDodga) {
                    return;
                }
                isStartDodgaDownTime = true;
            }
            dodgaLimitTime -= deltaTime;
        }

        private void CountDodgeTime(float deltaTime) {
            dodgaTime -= deltaTime;
            if (dodgaTime <= 0) {
                CancelDodga();
            }
        }

        public void OnJumpCallBack(CM_InputPlayer.Jump ent) {
            CancelDodga();
        }

        private bool CanUseDodga() {
            return dodgaLimitTime <= 0.0f;
        }

        public void OnDodgaCallBack(CM_InputPlayer.Dodge ent) {
            if (CanUseDodga() == false) {
                return;
            }
            dodgaLimitTime = 0.5f;
            dodgaTime = 0.4f;
            isDodga = true;
            isStartDodgaDownTime = false;
            mySystem.Dispatcher(new CE_Character.Dodga {
                IsDodge = true
            });
        }

        private void CancelDodga() {
            if (isDodga == false) {
                return;
            }
            dodgaTime = 0.0f;
            isDodga = false;
            mySystem.Dispatcher(new CE_Character.Dodga {
                IsDodge = false
            });
        }
    }
}