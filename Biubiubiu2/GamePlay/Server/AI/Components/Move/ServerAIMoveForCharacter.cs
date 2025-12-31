using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 直线匀速移动
    public class ServerAIMoveForCharacter : ServerAIMove {
        public struct InitData : SystemBase.IComponentInitData {
            public float MoveSpeed;
            public float MoveRotaSpeed;
        }
        private float moveSpeed = 4f; // 跑步速度
        private float rotaSpeed = 60f;
        private bool isNavMeshSetDestination = false;
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private Vector3 moveAddForce = Vector3.zero;
        private S_AI_Base aiSystem;
        private Vector3 moveDir = Vector3.zero;
        private Vector3 movePoint = Vector3.zero;
        private Vector3 prevPoint = Vector3.zero;
        private RaycastHit[] raycastHit;
        private float moveDirY = 0f;
        private float groundDis = 0f;
        private float movePointProtectTime = 0f;
        private float abiityMoveSpeed = 0f;
        private float checkGroundDeltaTime = 0.0f;
        private float protectDeltaTime = 0.0f;
        private bool isGround = false;
        private bool isMovePoint = false;
        private bool isAbilityMove = false;
        private bool isKnockbackMove = false;
        private bool isStrikeFlyMove = false;
        private bool isHideEntity = false;
        private GPOData.GPOType driveState = GPOData.GPOType.NULL;
        private float deltaCheckMoveDir = 0.0f;
        private bool isDead = false;
        private float delayGetNavMeshPointTime = 0.0f;

        /// <summary>
        ///  跳跃相关
        /// </summary>
        private const float jumpHight = 1.5f; // 跳跃到 1.5 米的高度

        private const float airJumpHight = 1.0f; // 二段跳额外提升的高度
        private const float jumpTime = 1.2f; // 多少时间跳跃到目标高度
        private const float weight = 30f; // 重量
        private const float jumpPower = 15f; // 跳跃力量
        private float jumpEndPy = 0.0f;
        private float startJumpTime = 0.0f;
        private CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        private bool isJump = false;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Register<SE_Ability.MovePoint>(SetMovePointForAbility);
            mySystem.Register<SE_Ability.CanceMovePoint>(CanceMovePointForAbility);
            mySystem.Register<SE_Behaviour.Event_StopMove>(BehaviourStopMove);
            mySystem.Register<SE_GPO.Event_KnockbackMovePoint>(SetKnockbackMovePoint);
            mySystem.Register<SE_GPO.Event_StrikeFlyMovePoint>(SetStrikeFlyMovePoint);
            mySystem.Register<SE_Entity.Event_IsShowEntity>(SetHideEntity);
            mySystem.Register<SE_GPO.Event_JumpTypeChange>(OnJumpCallBack);
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
            mySystem.Register<SE_AI.Event_DriveState>(OnDriveStateCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            aiSystem = (S_AI_Base)mySystem;
            var initData = (InitData)initDataBase;
            this.moveSpeed = initData.MoveSpeed;
            this.rotaSpeed = initData.MoveRotaSpeed;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Unregister<SE_Ability.MovePoint>(SetMovePointForAbility);
            mySystem.Unregister<SE_Ability.CanceMovePoint>(CanceMovePointForAbility);
            mySystem.Unregister<SE_Behaviour.Event_StopMove>(BehaviourStopMove);
            mySystem.Unregister<SE_GPO.Event_KnockbackMovePoint>(SetKnockbackMovePoint);
            mySystem.Unregister<SE_GPO.Event_StrikeFlyMovePoint>(SetStrikeFlyMovePoint);
            mySystem.Unregister<SE_Entity.Event_IsShowEntity>(SetHideEntity);
            mySystem.Unregister<SE_AI.Event_DriveState>(OnDriveStateCallBack);
            mySystem.Unregister<SE_GPO.Event_JumpTypeChange>(OnJumpCallBack);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
            controller = null;
            raycastHit = null;
            aiSystem = null;
        }

        private void OnUpdate(float delta) {
            if (ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver) {
                StopMove();
                RemoveUpdate(OnUpdate);
                return;
            }
            UpdateEnableNavMesh();
            Move(delta);
            CheckGrounded();
            CheckMovePoint();
        }

        /// <summary>
        /// 被驾驶状态改变
        /// </summary>
        /// <param name="ent"></param>
        private void OnDriveStateCallBack(ISystemMsg body, SE_AI.Event_DriveState ent) {
            driveState = ent.DriveGpoType;
            if (driveState == GPOData.GPOType.Role) {
                if (navMeshAgent == null) {
                    return;
                }
            }
            var isEnable = driveState != GPOData.GPOType.Role;
            if (controller != null) {
                controller.enabled = isEnable;
            }
            if (isEnable == false) {
                StopMove();
            }
        }

        private void OnDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            isDead = ent.IsDead;
            if (isDead == false) {
                ResetNavMeshAgent();
            }
        }

        private void UpdateEnableNavMesh() {
            if (isMovePoint == false || driveState == GPOData.GPOType.Role || navMeshAgent == null) {
                if (navMeshAgent.enabled) {
                    navMeshAgent.enabled = false;
                }
            } else {
                if (navMeshAgent.enabled == false) {
                    navMeshAgent.enabled = true;
                }
            }
        }

        private void ResetNavMeshAgent() {
            navMeshAgent.enabled = false;
            navMeshAgent.enabled = true;
        }

        private void BehaviourStopMove(ISystemMsg body, SE_Behaviour.Event_StopMove ent) {
            if (isAbilityMove || isHideEntity || driveState == GPOData.GPOType.Role) {
                return;
            }
            StopMove();
        }

        private void StopMove() {
            if (isMovePoint == true) {
                Dispatcher(new SE_AI.Event_IsMovePoint {
                    IsTrue = false
                });
                Dispatcher(new SE_GPO.Event_MovePointEnd());
            }
            this.movePoint = iEntity.GetPoint();
            isMovePoint = false;
        }

        public void SetMovePointForAbility(ISystemMsg body, SE_Ability.MovePoint entData) {
            if (isHideEntity) {
                return;
            }
            isAbilityMove = true;
            isKnockbackMove = false;
            isStrikeFlyMove = false;
            abiityMoveSpeed = entData.moveSpeed;
            SetMovePoint(entData.movePoint, AIData.MoveType.Run);
        }

        public void SetMovePointForAction(ISystemMsg body, SE_Behaviour.Event_MovePoint entData) {
            SetMovePoint(entData.movePoint, entData.MoveType);
        }

        public void SetKnockbackMovePoint(ISystemMsg body, SE_GPO.Event_KnockbackMovePoint entData) {
            if (isAbilityMove || isStrikeFlyMove || isHideEntity || IsControllerEnabled() == false) {
                isKnockbackMove = false;
                return;
            }
            if (entData.KnockbackMove == Vector3.zero) {
                isKnockbackMove = false;
            } else {
                isKnockbackMove = true;
                ControllerMove(entData.KnockbackMove);
            }
        }

        public void SetStrikeFlyMovePoint(ISystemMsg body, SE_GPO.Event_StrikeFlyMovePoint ent) {
            if (isAbilityMove || isHideEntity || IsControllerEnabled() == false) {
                isStrikeFlyMove = false;
                return;
            }
            if (ent.StrikeFlyMove == Vector3.zero) {
                isStrikeFlyMove = false;
            } else {
                isStrikeFlyMove = true;
                ControllerMove(ent.StrikeFlyMove);
            }
        }

        private bool IsControllerEnabled() {
            if (this.controller == null) {
                return false;
            }
            return this.controller.enabled;
        }

        private void SetHideEntity(ISystemMsg body, SE_Entity.Event_IsShowEntity ent) {
            isHideEntity = !ent.IsShow;
            StopMove();
        }

        private void CanceMovePointForAbility(ISystemMsg body, SE_Ability.CanceMovePoint ent) {
            if (isAbilityMove) {
                isAbilityMove = false;
                StopMove();
            }
        }

        /// <summary>
        /// 输入需要移动到的目的地坐标
        /// </summary>
        /// <param name="movePoint"></param>
        public void SetMovePoint(Vector3 movePoint, AIData.MoveType moveType) {
            if (this.movePoint == movePoint || isDead) {
                return;
            }
            if (driveState == GPOData.GPOType.Role) {
                return;
            }
            this.movePoint = movePoint;
            isMovePoint = true;
            moveDir = (movePoint - iEntity.GetPoint()).normalized;
            this.moveType = moveType;
            isNavMeshSetDestination = false;
            Dispatcher(new SE_AI.Event_IsMovePoint {
                IsTrue = true, MoveType = moveType
            });
        }

        private void Move(float deltaTime) {
            if (isMovePoint == false || isStrikeFlyMove || isKnockbackMove || isHideEntity ||
                IsControllerEnabled() == false) {
                return;
            }
            CheckMoveDir();
#if UNITY_EDITOR
            if (navMeshAgent.isOnNavMesh) {
                Debug.DrawLine(iEntity.GetPoint(), (iEntity.GetPoint() + moveDir * 10), Color.yellow);
                Debug.DrawLine(iEntity.GetPoint(), (iEntity.GetPoint() + Vector3.up * 10), Color.yellow);
            } else {
                Debug.DrawLine(iEntity.GetPoint(), (iEntity.GetPoint() + Vector3.up * 10), Color.red);
            }
            Debug.DrawLine(iEntity.GetPoint(), movePoint, Color.green);
#endif
            DelayGetNavMeshPoint();
            var useDir = Vector3.zero;
            MoveRota();
            if (isJump) {
                JumpMove();
            } else {
                FallMove(deltaTime, weight);
            }
            var speed = GetSpeed();
            var forward = iEntity.GetForward();
            if (this.moveType == AIData.MoveType.BackMove) {
                forward *= -1;
            }
            useDir.x = forward.x * speed;
            useDir.z = forward.z * speed;
            useDir.y = moveDirY;
            navMeshAgent.speed = speed;
            ControllerMove((useDir + moveAddForce) * deltaTime);
            navMeshAgent.nextPosition = iEntity.GetPoint();
            var distance = Vector3.Distance(iEntity.GetPoint(), movePoint);
            if (distance <= 0.5f) {
                StopMove();
            }
        }
        private void JumpMove() {
            var diffJumpValue = jumpEndPy - iEntity.GetPoint().y;
            var velocityY = this.controller.velocity.y;
            var diffJumpStartTime = Time.time - startJumpTime;
            if (velocityY <= 0.01f && diffJumpStartTime > 0.1f) {
                moveDirY = 0f;
            } else {
                moveDirY = diffJumpValue / jumpTime * jumpPower;
            }
            if (moveDirY <= 0.5f) {
                isJump = false;
                mySystem.Dispatcher(new SE_GPO.Event_Fall {
                    FallValue = moveDirY
                });
            }
        }

        // 检查角色是否被卡在某个坐标无法正常往前移动
        private void CheckMovePoint() {
            if (protectDeltaTime < 1f) {
                protectDeltaTime += Time.deltaTime;
                return;
            }
            protectDeltaTime = 0f;
            if (isMovePoint == false) {
                movePointProtectTime = 0.0f;
                return;
            }
            var distance = Vector3.Distance(iEntity.GetPoint(), prevPoint);
            prevPoint = iEntity.GetPoint();
            if (distance <= 0.1f) {
                movePointProtectTime += Time.deltaTime;
                if (movePointProtectTime >= 5f) {
                    StopMove();
                }
            } else {
                movePointProtectTime = 0.0f;
            }
        }

        /// <summary>
        /// 获取下一个移动方向
        /// </summary>
        private void CheckMoveDir() {
            var nowPoint = iEntity.GetPoint();
            nowPoint.y = movePoint.y;
            if (navMeshAgent.enabled == false || navMeshAgent.isOnNavMesh == false) {
                moveDir = (movePoint - nowPoint).normalized;
            } else {
                if (isNavMeshSetDestination == false) {
                    navMeshAgent.SetDestination(this.movePoint);
                    isNavMeshSetDestination = true;
                }
                moveDir = navMeshAgent.desiredVelocity.normalized;
            }
            Dispatcher(new SE_GPO.Event_MoveDir {
                MoveDir = moveDir
            });
        }

        /// <summary>
        /// 根据目标朝向进行旋转
        /// </summary>
        private void MoveRota() {
            if (moveDir == Vector3.zero) {
                return;
            }
            var targetRotation =
                Quaternion.LookRotation(this.moveType == AIData.MoveType.BackMove ? -moveDir : moveDir,
                    Vector3.up);
            var distance = (GetRotaSpeed() - (movePoint - iEntity.GetPoint()).magnitude) * 0.1f;
            if (distance < 1) {
                distance = 1;
            }
            // **平滑插值旋转**
            var newRotation = Quaternion.Slerp(iEntity.GetRota(), targetRotation, distance * Time.deltaTime);
            // **更新旋转**
            iEntity.SetRota(newRotation);
        }
        
        private float GetRotaSpeed() {
            if (moveType == AIData.MoveType.Run) {
                return rotaSpeed * 1.2f;
            }
            return rotaSpeed;
        }

        private float GetSpeed() {
            if (isAbilityMove) {
                return abiityMoveSpeed;
            } else if (moveType == AIData.MoveType.Run) {
                return moveSpeed * 1.2f;
            }
            return moveSpeed;
        }

        private void FallMove(float deltaTime, float weight) {
            if (moveDirY >= 0) {
                moveDirY = 0f;
            }
            moveDirY -= gravitey * weight * deltaTime;
            var maxDownGravitey = -weight * gravitey;
            if (groundDis >= 0f && groundDis <= 1f) {
                maxDownGravitey *= groundDis;
            }
            if (moveDirY <= maxDownGravitey) {
                moveDirY = maxDownGravitey;
            }
            if (isGround) {
                if (jumpType == CharacterData.JumpType.Fall) {
                    mySystem.Dispatcher(new SE_GPO.Event_FallToGrounded());
                }
            } else {
                mySystem.Dispatcher(new SE_GPO.Event_Fall {
                    FallValue = moveDir.y
                });
            }
        }

        private void CheckGrounded() {
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

        public void IsGround(bool isTrue, float groundDis) {
            this.groundDis = groundDis;
            this.mySystem.Dispatcher(new SE_GPO.Event_GroundDistance {
                IsTrue = isTrue, GroundDis = groundDis
            });
            if (isGround != isTrue) {
                isGround = isTrue;
                this.mySystem.Dispatcher(new SE_GPO.Event_IsGround {
                    IsTrue = isTrue
                });
            }
        }

        public void OnJumpCallBack(ISystemMsg body, SE_GPO.Event_JumpTypeChange envData) {
            var jumpType = envData.JumpType;
            if (jumpType == CharacterData.JumpType.Jump) {
                jumpEndPy = iEntity.GetPoint().y + jumpHight / gravitey;
            } else if (jumpType == CharacterData.JumpType.AirJump) {
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

        private void CancelJumpEndY() {
            jumpEndPy = iEntity.GetPoint().y;
            moveDir.y = 0f;
            isJump = false;
        }
    }
}