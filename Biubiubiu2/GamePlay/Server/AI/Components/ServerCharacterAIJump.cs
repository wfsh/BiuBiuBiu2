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
    public class ServerCharacterAIJump : ServerNetworkComponentBase {
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
            this.mySystem.Register<SE_Behaviour.Event_OnJump>(OnJumpCallBack);
            this.mySystem.Register<SE_Behaviour.Event_OnAirJump>(OnAirJumpCallBack);
            this.mySystem.Register<SE_GPO.Event_GroundDistance>(OnGroundDistanceCallBack);
            this.mySystem.Register<SE_GPO.Event_Fall>(OnFallCallBack);
            this.mySystem.Register<SE_GPO.Event_StrikeFlyMovePoint>(OnStrikeFlyMovePointCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            CancelFly();
            this.mySystem.Unregister<SE_Behaviour.Event_OnJump>(OnJumpCallBack);
            this.mySystem.Unregister<SE_Behaviour.Event_OnAirJump>(OnAirJumpCallBack);
            this.mySystem.Unregister<SE_GPO.Event_GroundDistance>(OnGroundDistanceCallBack);
            this.mySystem.Unregister<SE_GPO.Event_Fall>(OnFallCallBack);
            this.mySystem.Unregister<SE_GPO.Event_StrikeFlyMovePoint>(OnStrikeFlyMovePointCallBack);
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

        private void OnFallCallBack(ISystemMsg body, SE_GPO.Event_Fall evnData) {
            if (evnData.FallValue <= -1f && flyType == CharacterData.FlyType.None) {
                SetJumpType(CharacterData.JumpType.Fall);
            }
        }

        private void OnGroundDistanceCallBack(ISystemMsg body, SE_GPO.Event_GroundDistance evnData) {
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

        private void CancelJump() {
            SetJumpType(CharacterData.JumpType.None);
            jumpCount = 0;
        }

        public void OnJumpCallBack(ISystemMsg body, SE_Behaviour.Event_OnJump ent) {
            OnJump();
        }

        public void OnAirJumpCallBack(ISystemMsg body, SE_Behaviour.Event_OnAirJump ent) {
            OnJump();
            UpdateRegister.AddInvoke(OnJump, Random.Range(0.1f, 0.5f));
        }

        public void OnJump() {
            if (isClear) {
                return;
            }
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
            }
        }

        private void OnStrikeFlyMovePointCallBack(ISystemMsg body, SE_GPO.Event_StrikeFlyMovePoint ent) {
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
            mySystem.Dispatcher(new SE_GPO.Event_JumpTypeChange {
                JumpType = jumpType
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
            mySystem.Dispatcher(new SE_GPO.Event_FlyTypeChange() {
                FlyType = flyType
            });
        }
    }
}