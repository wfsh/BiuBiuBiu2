using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterInputToJumpState : ClientCharacterComponent {
        private float OpenParachuteDistance = 3f;
        private float CloseParachuteDistance = 1.5f;
        private CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        private CharacterData.FlyType flyType = CharacterData.FlyType.None;
        private int jumpCount = 0;
        private bool isInSlipRope = false;
        private bool isGround = false;
        private float openParachute = 0.0f;
        private float groundDistance = 0.0f;
        private float checkJumpToGroundTime = 0.0f;
        private bool isUseAerocraft = false;
        private CharacterData.WirebugType wirebugType = CharacterData.WirebugType.None;

        protected override void OnAwake() {
            MsgRegister.Register<CM_InputPlayer.Jump>(OnJumpCallBack);
            this.mySystem.Register<CE_Ability.InSlipRope>(OnInSlipRopeCallBack);
            this.mySystem.Register<CE_Character.GroundDistance>(OnGroundDistanceCallBack);
            this.mySystem.Register<CE_Character.Fall>(OnFallCallBack);
            this.mySystem.Register<CE_Character.Dodga>(OnDodgaCallBack);
            this.mySystem.Register<CE_Character.PlayAttackAnim>(OnPlayAttackAnimCallBack);
            this.mySystem.Register<CE_Character.Event_WirebugMoveState>(OnWirebugMoveStateBack);
            this.mySystem.Register<CE_Character.Event_StrikeFlyMovePoint>(OnStrikeFlyMovePointCallBack);
            this.mySystem.Register<CE_Character.UseAerocraft>(OnUseAerocraftCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            CancelFly();
            MsgRegister.Unregister<CM_InputPlayer.Jump>(OnJumpCallBack);
            this.mySystem.Unregister<CE_Ability.InSlipRope>(OnInSlipRopeCallBack);
            this.mySystem.Unregister<CE_Character.GroundDistance>(OnGroundDistanceCallBack);
            this.mySystem.Unregister<CE_Character.Dodga>(OnDodgaCallBack);
            this.mySystem.Unregister<CE_Character.Fall>(OnFallCallBack);
            this.mySystem.Unregister<CE_Character.PlayAttackAnim>(OnPlayAttackAnimCallBack);
            this.mySystem.Unregister<CE_Character.Event_StrikeFlyMovePoint>(OnStrikeFlyMovePointCallBack);
            this.mySystem.Unregister<CE_Character.Event_WirebugMoveState>(OnWirebugMoveStateBack);
            this.mySystem.Unregister<CE_Character.UseAerocraft>(OnUseAerocraftCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            if (flyType == CharacterData.FlyType.OpenParachute) {
                this.openParachute -= Time.deltaTime;
                if (this.openParachute <= 0) {
                    SetFlyType(CharacterData.FlyType.Fly);
                }
            }
        }

        private void OnUseAerocraftCallBack(ISystemMsg body, CE_Character.UseAerocraft ent) {
            isUseAerocraft = ent.useAerocraft != "";
        }

        private void OnFallCallBack(ISystemMsg body, CE_Character.Fall evnData) {
            if (evnData.FallValue <= -3f && flyType == CharacterData.FlyType.None) {
                SetJumpType(CharacterData.JumpType.Fall);
            }
        }

        private void OnGroundDistanceCallBack(ISystemMsg body, CE_Character.GroundDistance evnData) {
            isGround = evnData.IsTrue;
            groundDistance = evnData.GroundDis;
            if (isGround || evnData.GroundDis <= CloseParachuteDistance) {
                CancelFly();
            }
            if (Time.realtimeSinceStartup - checkJumpToGroundTime >= 0.2f || CharacterData.IsJump(jumpType) == false) {
                if (isGround) {
                    CancelJump();
                }
            }
        }

        private void OnInSlipRopeCallBack(ISystemMsg body, CE_Ability.InSlipRope entData) {
            isInSlipRope = entData.isInSlipRope;
            if (isInSlipRope == false) {
                if (CharacterData.IsJump(jumpType) == false) {
                    OnJump();
                }
            }
        }

        private void CancelJump() {
            SetJumpType(CharacterData.JumpType.None);
            jumpCount = 0;
        }

        public void OnJumpCallBack(CM_InputPlayer.Jump ent) {
            OnJump();
        }

        public void OnJump() {
            checkJumpToGroundTime = Time.realtimeSinceStartup;
            if (jumpCount >= 2) {
                jumpCount = 2;
                if (CharacterData.IsFly(flyType)) {
                    CancelFly();
                } else {
                    if (isUseAerocraft == true && groundDistance >= OpenParachuteDistance) {
                        SetFlyType(CharacterData.FlyType.OpenParachute);
                        SetJumpType(CharacterData.JumpType.None);
                    }
                }
            } else {
                if (jumpCount == 0 && isGround == false) {
                    jumpCount++;
                }
                jumpCount++;
                if (jumpCount == 1) {
                    SetJumpType(CharacterData.JumpType.Jump);
                } else {
                    SetJumpType(CharacterData.JumpType.AirJump);
                }
                if (isInSlipRope) {
                    this.mySystem.Dispatcher<CE_Ability.OutSlipRope>(new CE_Ability.OutSlipRope() {
                        gpoId = this.iGPO.GetGpoID()
                    });
                }
            }
        }

        private void OnPlayAttackAnimCallBack(ISystemMsg body, CE_Character.PlayAttackAnim ent) {
            if (ent.PlayAnimId == 0) {
                return;
            }
            CancelFly();
            CancelJump();
        }

        private void OnStrikeFlyMovePointCallBack(ISystemMsg body, CE_Character.Event_StrikeFlyMovePoint ent) {
            CancelFly();
            CancelJump();
        }

        private void OnWirebugMoveStateBack(ISystemMsg body, CE_Character.Event_WirebugMoveState ent) {
            wirebugType = ent.State;
            CancelFly();
            CancelJump();
        }
        
        private void CancelFly() {
            SetFlyType(CharacterData.FlyType.None);
        }

        private void SetJumpType(CharacterData.JumpType jumpType) {
            if (this.jumpType == jumpType) {
                return;
            }
            this.jumpType = jumpType;
            mySystem.Dispatcher(new CE_Character.JumpTypeChange {
                JumpType = jumpType
            });
            Cmd(new Proto_Character.Cmd_JumpType {
                jumpType = jumpType,
            });
        }

        private void SetFlyType(CharacterData.FlyType flyType) {
            if (this.flyType == flyType) {
                return;
            }
            if (flyType == CharacterData.FlyType.OpenParachute) {
                this.openParachute = 0.5f;
            }
            this.flyType = flyType;
            mySystem.Dispatcher(new CE_Character.FlyTypeChange() {
                FlyType = flyType
            });
            Cmd(new Proto_Character.Cmd_FlyType {
                flyType = flyType,
            });
        }

        public void OnDodgaCallBack(ISystemMsg body, CE_Character.Dodga envData) {
            if (envData.IsDodge) {
                CancelFly();
                CancelJump();
            } else {
                if (isGround == false) {
                    SetJumpType(CharacterData.JumpType.Fall);
                }
            }
        }
    }
}