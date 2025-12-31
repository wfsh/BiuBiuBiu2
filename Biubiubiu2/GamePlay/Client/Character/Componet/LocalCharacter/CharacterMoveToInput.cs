using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterMoveToInput : ClientCharacterComponent {
        private const float speed = 7f; // 移动速度
        private const float gravitey = 1f; // 影响下落速度和跳跃高度 默认 1 倍重力
        private const float jumpHight = 1.5f; // 跳跃到 1.5 米的高度
        private const float airJumpHight = 1.0f; // 二段跳额外提升的高度
        private const float jumpTime = 1.2f; // 多少时间跳跃到目标高度
        private const float weight = 30f; // 重量
        private const float jumpPower = 15f; // 跳跃力量
        private CharacterController controller;
        private CharacterEntity cEntity;
        private CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        private CharacterData.FlyType flyType = CharacterData.FlyType.None;
        private CharacterData.StandType standType = 0;
        private Vector3 moveDir = Vector3.zero;
        private Vector3 moveAddForce = Vector3.zero;
        private float jumpEndPy = 0.0f;
        private float startJumpTime = 0.0f;
        private float updraftPower = 0.0f;
        private float ability_SpeedRatio = 0.0f;
        private float groundDis = 0f;
        private bool isDodga = false;
        private bool isGround = false;
        private bool isInSlipRope = false;
        private bool isKnockbackMove = false;
        private bool isStrikeFlyMove = false;
        private bool isWirebugMove = false;
        private bool isSlide = false;
        private bool isAttackMove = false;
        private bool isStopDefaultMoveForAttack = false;
        private RaycastHit[] raycastHit;
        private float flySpeed = 0f;
        private float flyWeight = 0f;
        private bool isJump = false;
        private bool isDrive = false;
        private bool isDead = false;
        private IWeapon useWeapon;
        private float gunMoveSpeed = 0f;
        
        protected override void OnAwake() {
            MsgRegister.Register<CM_InputPlayer.Move>(OnMoveCallBack);
            MsgRegister.Register<CM_InputPlayer.MoveCanceled>(OnMoveCanceledCallBack);
            this.mySystem.Register<CE_Ability.InSlipRope>(OnInSlipRopeCallBack);
            this.mySystem.Register<CE_Ability.PlatformMovement>(OnPlatformMovementCallBack);
            this.mySystem.Register<CE_Ability.SlipRopeMove>(OnSlipRopeMoveCallBack);
            this.mySystem.Register<CE_GPO.Event_PlayerDriveGPO>(OnDriveIngCallBack);
            this.mySystem.Register<CE_Character.Event_KnockbackMovePoint>(OnKnockbackMovePointCallBack);
            this.mySystem.Register<CE_Character.Event_StrikeFlyMovePoint>(OnStrikeFlyMovePointCallBack);
            this.mySystem.Register<CE_Character.Event_AttackMovePoint>(OnAttackMovePointCallBack);
            this.mySystem.Register<CE_Character.Event_WirebugMove>(OnWirebugMoveCallBack);
            this.mySystem.Register<CE_Character.Event_SlideMove>(OnSlideMoveCallBack);
            this.mySystem.Register<CE_Character.StandTypeChange>(OnStandTypeCallBack);
            this.mySystem.Register<CE_Character.JumpTypeChange>(OnJumpCallBack);
            this.mySystem.Register<CE_Character.FlyTypeChange>(OnFlyCallBack);
            this.mySystem.Register<CE_Character.FlyMoveData>(OnFlyMoveDataCallBack);
            this.mySystem.Register<CE_Character.FlyTypeChange>(OnFlyCallBack);
            this.mySystem.Register<CE_Character.Dodga>(OnDodgaCallBack);
            this.mySystem.Register<CE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            this.mySystem.Register<CE_Character.Event_StopDefaultMoveForAttack>(OnStopDefaultMoveForAttackBack);
            this.mySystem.Register<CE_Weapon.UseWeapon>(OnUseWeaponCallBack);
            this.mySystem.Register<CE_AbilityEffect.Event_UpdateEffect>(OnUpdateEffectCallBack);
            raycastHit = new RaycastHit[10];
        }

        protected override void OnSetEntityObj(IEntity entity) {
            base.OnSetEntityObj(entity);
            cEntity = (CharacterEntity)iEntity;
            this.controller = cEntity.characterController;
            EnabledCharacterController();
            AddUpdate(Update);
        }

        private void OnSetIsDeadCallBack(ISystemMsg body, CE_GPO.Event_SetIsDead ent) {
            this.isDead = ent.IsDead;
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Character.TargetRpc_MovePoint.ID, OnTargetRpcMovePointCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            useWeapon = null;
            raycastHit = null;
            RemoveUpdate(Update);
            this.mySystem.Unregister<CE_Ability.InSlipRope>(OnInSlipRopeCallBack);
            this.mySystem.Unregister<CE_Ability.PlatformMovement>(OnPlatformMovementCallBack);
            this.mySystem.Unregister<CE_Ability.SlipRopeMove>(OnSlipRopeMoveCallBack);
            this.mySystem.Unregister<CE_Character.Event_KnockbackMovePoint>(OnKnockbackMovePointCallBack);
            this.mySystem.Unregister<CE_Character.Event_StrikeFlyMovePoint>(OnStrikeFlyMovePointCallBack);
            this.mySystem.Unregister<CE_Character.Event_WirebugMove>(OnWirebugMoveCallBack);
            this.mySystem.Unregister<CE_Character.Event_SlideMove>(OnSlideMoveCallBack);
            this.mySystem.Unregister<CE_Character.StandTypeChange>(OnStandTypeCallBack);
            this.mySystem.Unregister<CE_Character.JumpTypeChange>(OnJumpCallBack);
            this.mySystem.Unregister<CE_Character.FlyTypeChange>(OnFlyCallBack);
            this.mySystem.Unregister<CE_Character.FlyMoveData>(OnFlyMoveDataCallBack);
            this.mySystem.Unregister<CE_Character.FlyTypeChange>(OnFlyCallBack);
            this.mySystem.Unregister<CE_Character.Dodga>(OnDodgaCallBack);
            this.mySystem.Unregister<CE_GPO.Event_PlayerDriveGPO>(OnDriveIngCallBack);
            this.mySystem.Unregister<CE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            this.mySystem.Unregister<CE_Character.Event_StopDefaultMoveForAttack>(OnStopDefaultMoveForAttackBack);
            this.mySystem.Unregister<CE_Weapon.UseWeapon>(OnUseWeaponCallBack);
            this.mySystem.Unregister<CE_AbilityEffect.Event_UpdateEffect>(OnUpdateEffectCallBack);
            MsgRegister.Unregister<CM_InputPlayer.Move>(OnMoveCallBack);
            MsgRegister.Unregister<CM_InputPlayer.MoveCanceled>(OnMoveCanceledCallBack);
            this.RemoveProtoCallBack(Proto_Character.TargetRpc_MovePoint.ID, OnTargetRpcMovePointCallBack);
            cEntity = null;
            this.controller = null;
        }

        public void OnUpdateEffectCallBack(ISystemMsg body, CE_AbilityEffect.Event_UpdateEffect env) {
            switch (env.Effect) {
                case AbilityEffectData.Effect.UpdraftPoint:
                    updraftPower = env.Value;
                    break;
                case AbilityEffectData.Effect.GpoMoveSpeedRate:
                    ability_SpeedRatio = env.Value;
                    break;
            }
        }

        private void OnTargetRpcMovePointCallBack(INetwork network, IProto_Doc proto) {
            if (isDrive) {
                return;
            }
            var data = (Proto_Character.TargetRpc_MovePoint)proto;
            iEntity.SetPoint(data.Point);
            iEntity.SetRota(data.Rotation);
            mySystem.Dispatcher(new CE_GPO.ServerSetPoint {
                Point = data.Point,
                Rota = data.Rotation
            });
        }

        private void OnDriveIngCallBack(ISystemMsg body, CE_GPO.Event_PlayerDriveGPO ent) {
            isDrive = ent.DriveGPO != null;
            EnabledCharacterController();
        }
        
        private void EnabledCharacterController() {
            if (this.controller == null) {
                return;
            }
            this.controller.enabled = isDrive == false;
        }

        public void OnMoveCallBack(CM_InputPlayer.Move ent) {
            moveDir.x = ent.Px;
            moveDir.z = ent.Pz;
            this.mySystem.Dispatcher(new CE_GPO.Event_MoveDir() {
                MoveH = ent.Px, MoveV = ent.Pz
            });
        }

        public void OnMoveCanceledCallBack(CM_InputPlayer.MoveCanceled ent) {
            if (isDrive) {
                return;
            }
            moveDir.x = 0f;
            moveDir.z = 0f;
            this.mySystem.Dispatcher(new CE_GPO.Event_MoveDir() {
                MoveH = 0f, MoveV = 0f
            });
        }

        private void OnFlyMoveDataCallBack(ISystemMsg body, CE_Character.FlyMoveData envData) {
            this.flySpeed = envData.FlySpeed;
            this.flyWeight = envData.FlyWeight;
        }

        public void Update(float deltaTime) {
            MoveCheck(deltaTime);
            PerfAnalyzerAgent.SetPointAndRota(iEntity.GetPoint(), iEntity.GetEulerAngles());
        }

        private void MoveCheck(float deltaTime) {
            if (isDrive || this.isDead) {
                return;
            }
            if (isStrikeFlyMove == false &&
                isAttackMove == false && 
                isWirebugMove == false &&
                isSlide == false &&
                isStopDefaultMoveForAttack == false) {
                Move(deltaTime);
            }
            CheckGrounded();
        }

        private void Move(float deltaTime) {
            var useDir = Vector3.zero;
            if (CharacterData.IsFly(flyType)) {
                useDir = FlyUpdate(deltaTime);
            } else if (isDodga) {
                useDir = DodgaMove();
            } else {
                if (isJump) {
                    JumpMove();
                } else {
                    FallMove(deltaTime, weight);
                }
                if (isInSlipRope && isJump == false) {
                    useDir.y = moveDir.y;
                } else {
                    var speed = GetSpeed();
                    var ratio = 1f;
                    if (moveDir.x != 0 || moveDir.z != 0) {
                        ratio = (1f / (Mathf.Abs(moveDir.x) + Mathf.Abs(moveDir.z)));
                    }
                    useDir.x = moveDir.x * speed * ratio;
                    useDir.z = moveDir.z * speed * ratio;
                    useDir.y = moveDir.y;
                }
            }
            var tranDir = cEntity.TransformDirection(useDir);
            this.controller.Move((tranDir + moveAddForce) * deltaTime);
            EventDownMoveAddForce(deltaTime);
        }

        private void EventDownMoveAddForce(float deltaTime) {
            if (moveAddForce != Vector3.zero) {
                var dir = Vector3.Distance(moveAddForce, Vector3.zero);
                if (dir <= 0.5f) {
                    moveAddForce = Vector3.zero;
                } else {
                    moveAddForce = Vector3.MoveTowards(moveAddForce, Vector3.zero, deltaTime * 6f);
                }
            }
        }


        private Vector3 FlyUpdate(float deltaTime) {
            var useDir = Vector3.zero;
            var speed = GetSpeed();
            if (updraftPower > 0) {
                useDir = UpdraftMove(deltaTime);
            } else {
                FallMove(deltaTime, flyWeight);
                useDir.x = moveDir.x * speed;
                useDir.z = moveDir.z * speed;
                useDir.y = moveDir.y;
            }
            return useDir;
        }

        private Vector3 DodgaMove() {
            var useDir = Vector3.zero;
            var speed = GetSpeed();
            if (moveDir.x == 0 && moveDir.z == 0) {
                useDir.x = 0f;
                useDir.z = speed;
            } else {
                useDir.x = moveDir.x * speed;
                useDir.z = moveDir.z * speed;
            }
            return useDir;
        }

        private void JumpMove() {
            var diffJumpValue = jumpEndPy - iEntity.GetPoint().y;
            var velocityY = this.controller.velocity.y;
            var diffJumpStartTime = Time.time - startJumpTime;
            if (velocityY <= 0.01f && diffJumpStartTime > 0.1f) {
                moveDir.y = 0f;
            } else {
                moveDir.y = diffJumpValue / jumpTime * jumpPower;
            }
            if (moveDir.y <= 0.5f) {
                isJump = false;
                mySystem.Dispatcher(new CE_Character.Fall {
                    FallValue = moveDir.y
                });
            }
        }

        private void OnInSlipRopeCallBack(ISystemMsg body, CE_Ability.InSlipRope entData) {
            isInSlipRope = entData.isInSlipRope;
        }

        private void OnKnockbackMovePointCallBack(ISystemMsg body, CE_Character.Event_KnockbackMovePoint entData) {
            if (isStrikeFlyMove || isWirebugMove || isDrive || isSlide) {
                isKnockbackMove = false;
                return;
            }
            if (entData.KnockbackMove == Vector3.zero) {
                isKnockbackMove = false;
            } else {
                isKnockbackMove = true;
                this.controller?.Move(entData.KnockbackMove);
            }
        }

        private void OnStrikeFlyMovePointCallBack(ISystemMsg body, CE_Character.Event_StrikeFlyMovePoint entData) {
            if (entData.StrikeFlyMove == Vector3.zero || isDrive|| isSlide) {
                isStrikeFlyMove = false;
            } else {
                isStrikeFlyMove = true;
                AttackCanceJump();
                this.controller?.Move(entData.StrikeFlyMove);
            }
        }

        private void OnWirebugMoveCallBack(ISystemMsg body, CE_Character.Event_WirebugMove entData) {
            if (entData.WirebugMove == Vector3.zero|| isDrive) {
                isWirebugMove = false;
            } else {
                isWirebugMove = true;
                AttackCanceJump();
                this.controller?.Move(entData.WirebugMove);
            }
        }

        private void OnSlideMoveCallBack(ISystemMsg body, CE_Character.Event_SlideMove ent) {
            isSlide = ent.IsSlide;
            if (isSlide == false || isDrive) {
                moveAddForce = Vector3.zero;
            } else {
                moveAddForce = ent.SlideVelocity;
                AttackCanceJump();
                this.controller?.Move(ent.SlideVelocity);
            }
        }
        
        private void OnAttackMovePointCallBack(ISystemMsg body, CE_Character.Event_AttackMovePoint entData) {
            if (entData.AttackMove == Vector3.zero|| isDrive) {
                isAttackMove = false;
            } else {
                isAttackMove = true;
                AttackCanceJump();
                this.controller?.Move(entData.AttackMove);
            }
        }

        private void AttackCanceJump() {
            // mySystem.Dispatcher(new CE_Character.Fall {
            //     FallValue = -1,
            // });
            CancelJumpEndY();
        }

        private void OnStopDefaultMoveForAttackBack(ISystemMsg body, CE_Character.Event_StopDefaultMoveForAttack entData) {
            isStopDefaultMoveForAttack = entData.IsTrue;
        }

        private void OnPlatformMovementCallBack(ISystemMsg body, CE_Ability.PlatformMovement entData) {
            // if (this.isInSlipRope || isDrive) {
            //     return;
            // }
            // if (entData.point != Vector3.zero) {
            //     this.controller.Move(entData.point);
            //     var elementEntity = (SceneElementEntity)entData.entity;
            //     var point = elementEntity.InverseTransformPoint(iEntity.GetPoint());
            //     // this.mySystem.Dispatcher(new CE_Character.PlatformMovement {
            //     //     elementId = elementEntity.ElementIndex, point = point
            //     // });
            // }
        }

        private void OnSlipRopeMoveCallBack(ISystemMsg body, CE_Ability.SlipRopeMove entData) {
            moveAddForce = entData.forward;
        }

        private Vector3 UpdraftMove(float deltaTime) {
            var useDir = Vector3.zero;
            moveDir.y = updraftPower * 15 * deltaTime;
            useDir.x = moveDir.x * flySpeed;
            useDir.z = moveDir.z * flySpeed;
            useDir.y = moveDir.y;
            return useDir;
        }

        private void FallMove(float deltaTime, float weight) {
            if (moveDir.y >= 0) {
                moveDir.y = 0f;
            }
            moveDir.y -= gravitey * weight * deltaTime;
            var maxDownGravitey = -weight * gravitey * 0.6f;
            if (groundDis >= 0f && groundDis <= 1f) {
                maxDownGravitey *= groundDis;
            }
            if (moveDir.y <= maxDownGravitey) {
                moveDir.y = maxDownGravitey;
            }
            if (isGround) {
                if (jumpType == CharacterData.JumpType.Fall) {
                    mySystem.Dispatcher(new CE_Character.FallToGrounded());
                }
            } else {
                mySystem.Dispatcher(new CE_Character.Fall {
                    FallValue = moveDir.y
                });
            }
        }

        private void CheckGrounded() {
            if (this.isInSlipRope) {
                IsGround(true, 0f);
                return;
            }
            if (this.controller.isGrounded) {
                IsGround(true, 0f);
                return;
            }
            var point = this.iEntity.GetPoint() + Vector3.up * 0.2f;
            var count = Physics.RaycastNonAlloc(point, Vector3.down, raycastHit, 3f);
            this.groundDis = -1f;
            GetGoundData(count, point, raycastHit);
            if (this.groundDis < 0f) {
                this.groundDis = 3f;
            }
            IsGround(this.groundDis <= 0.5f, this.groundDis);
        }

        public void IsGround(bool isTrue, float groundDis) {
            this.groundDis = groundDis;
            this.mySystem.Dispatcher(new CE_Character.GroundDistance {
                IsTrue = isTrue, GroundDis = groundDis
            });
            if (isGround != isTrue) {
                isGround = isTrue;
                this.mySystem.Dispatcher(new CE_Character.IsGround {
                    IsTrue = isTrue
                });
            }
        }

        private void GetGoundData(int count, Vector3 point, RaycastHit[] list) {
            for (int i = 0; i < count; i++) {
                var ray = list[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var dis = Vector3.Distance(point, ray.point);
                if (groundDis < 0f || groundDis > dis) {
                    groundDis = dis;
                }
            }
        }

        private float GetSpeed() {
            var useSpeed = speed;
            if (gunMoveSpeed > 0f) {
                useSpeed = gunMoveSpeed;
            } else if (isDodga) {
                useSpeed *= 3f;
            } else if (CharacterData.IsFly(flyType)) {
                useSpeed = flySpeed;
            } else if (CharacterData.IsCrouch(standType)) {
                useSpeed *= 0.5f;
            } else if (CharacterData.IsProne(standType)) {
                useSpeed *= 0.2f;
            }
            if (isDodga == false) {
                useSpeed *= 1 + ability_SpeedRatio;
            }
            return useSpeed;
        }

        public void OnStandTypeCallBack(ISystemMsg body, CE_Character.StandTypeChange envData) {
            this.standType = envData.StandType;
        }

        public void OnJumpCallBack(ISystemMsg body, CE_Character.JumpTypeChange envData) {
            var jumpType = envData.JumpType;
            if (jumpType == CharacterData.JumpType.Jump) {
                jumpEndPy = iEntity.GetPoint().y + jumpHight / gravitey;
            }else if (jumpType == CharacterData.JumpType.AirJump) {
                jumpEndPy = iEntity.GetPoint().y + airJumpHight / gravitey;
            }
            this.jumpType = jumpType;
            isJump = CharacterData.IsJump(jumpType);
            if (isJump == false) {
                CancelJumpEndY();
            } else {
                startJumpTime = Time.time;
            }
        }

        public void OnFlyCallBack(ISystemMsg body, CE_Character.FlyTypeChange envData) {
            this.flyType = envData.FlyType;
        }

        public void OnDodgaCallBack(ISystemMsg body, CE_Character.Dodga ent) {
            isDodga = ent.IsDodge;
            if (ent.IsDodge) {
                CancelJumpEndY();
            }
        }

        public void OnUseWeaponCallBack(ISystemMsg body, CE_Weapon.UseWeapon ent) {
            useWeapon = ent.weapon;
            gunMoveSpeed = 0f;
            if (useWeapon != null) {
                useWeapon.Dispatcher(new CE_Weapon.Event_GetGunMoveSpeed {
                    CallBack = value => {
                        gunMoveSpeed = value;
                    }
                });
            }
        }
        
        private void CancelJumpEndY() {
            jumpEndPy = iEntity.GetPoint().y;
            moveDir.y = 0f;
            isJump = false;
        }
    }
}