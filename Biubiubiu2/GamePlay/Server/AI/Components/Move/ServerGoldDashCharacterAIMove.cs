using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 直线匀速移动
    public class ServerGoldDashCharacterAIMove : ComponentBase {
        private const float gravity = 1f; // 影响下落速度和跳跃高度 默认 1 倍重力
        private const float reachDistance = 0.5f; // 成功抵达目标点的判定距离
        private const float jumpOverObstacleDuration = 1; // 翻越障碍物的持续时间
        private const float moveDirUpdateInterval = 0.5f; // 移动方向更新间隔
        private bool isNavMeshSetDestination = false;
        private CharacterController controller;
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private Vector3 moveAddForce = Vector3.zero;
        private NavMeshAgent navMeshAgent;
        private EntityBase entity;
        private S_AI_Base aiSystem;
        private Vector3 moveDir = Vector3.zero;
        private Vector3 movePoint = Vector3.zero;
        private Vector3 prevPoint = Vector3.zero;
        private RaycastHit[] groundCheckRaycastHit;
        private float moveDirY = 0f;
        private float groundDis = 0f;
        private float movePointProtectTime = 0f;
        private float abilityMoveSpeed = 0f;
        private float checkGroundDeltaTime = 0.0f;
        private float protectDeltaTime = 0.0f;
        private bool isGround = false;
        private bool isMovePoint = false;
        private bool isAbilityMove = false;
        private bool isKnockbackMove = false;
        private bool isStrikeFlyMove = false;
        private bool isHideEntity = false;
        private GPOData.GPOType driveState = GPOData.GPOType.NULL;
        private bool isSlide = false;
        private float deltaCheckMoveDir = 0.0f;
        private bool isDead = false;
        private bool isPlayerDrive = false;
        private IWeapon useWeapon;
        private float gunMoveSpeed = 0f;
        private float delayGetNavMeshPointTime = 0.0f;
        private float moveDirUpdateTime;
        private bool hasBehaviorInit;
        private float ability_SpeedRatio = 0.0f;

        private float navMeshSampleTolerance = 2; // NavMesh 采样长度
        private bool isKeepEyesOnGPO = false; // 是否对目标保持注视
        private IGPO gpoEyesOn; // 保持注视的目标
        private float jumpOverLastTime; // 翻越障碍物的剩余时间

        private bool isInAlertStatus;
        private bool isInFightStatus;

        /// <summary>
        ///  跳跃相关
        /// </summary>
        private const float jumpTime = 1.2f; // 多少时间跳跃到目标高度
        private const float weight = 30f; // 重量
        private const float jumpPower = 15f; // 跳跃力量
        private float jumpEndPy = 0.0f;
        private float startJumpTime = 0.0f;
        private CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        private bool isJump = false;
        private bool isLookTarget;
        private IGPO lookTarget;
        private MonsterBehavior goldDashBehavior;

        protected override void OnAwake() {
            hasBehaviorInit = false;
            mySystem.Register<SE_AI.Event_JumpOverMove>(JumpOverMove);
            mySystem.Register<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Register<SE_Ability.MovePoint>(SetMovePointForAbility);
            mySystem.Register<SE_Ability.CanceMovePoint>(CanceMovePointForAbility);
            mySystem.Register<SE_Behaviour.Event_StopMove>(BehaviourStopMove);
            mySystem.Register<SE_Behaviour.Event_SetKeepEyesOnGPO>(SetKeepEyesOnGPO);
            mySystem.Register<SE_GPO.Event_KnockbackMovePoint>(SetKnockbackMovePoint);
            mySystem.Register<SE_GPO.Event_StrikeFlyMovePoint>(SetStrikeFlyMovePoint);
            mySystem.Register<SE_Entity.Event_IsShowEntity>(SetHideEntity);
            mySystem.Register<SE_Entity.SyncPointAndRota>(SetSyncMovePoint);
            mySystem.Register<SE_GPO.Event_JumpTypeChange>(OnJumpCallBack);
            mySystem.Register<SE_GPO.Event_SlideMove>(OnSlideMoveCallBack);
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
            mySystem.Register<SE_AI.Event_DriveState>(OnDriveStateCallBack);
            mySystem.Register<SE_GPO.Event_PlayerDrive>(OnPlayerDriveCallBack);
            mySystem.Register<SE_GPO.UseWeapon>(OnUseWeaponCallBack);
            mySystem.Register<SE_Behaviour.Event_AfterBehaviorConfigInit>(AfterBehaviorConfigInitCallBack);
            mySystem.Register<SE_AbilityEffect.Event_UpdateEffect>(OnUpdateEffectCallBack);
            mySystem.Register<SE_Behaviour.Event_LookTarget>(OnLookTargetCallBack);
            MsgRegister.Register<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
            goldDashBehavior = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(iGPO.GetSign());
        }

        protected override void OnStart() {
            base.OnStart();
            aiSystem = (S_AI_Base)mySystem;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            entity = (EntityBase)iEntity;
            GetCharacterController();
            GetNavMeshAgent();
            AddUpdate(OnUpdate);
            groundCheckRaycastHit = new RaycastHit[10];
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_AI.Event_JumpOverMove>(JumpOverMove);
            mySystem.Unregister<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Unregister<SE_Ability.MovePoint>(SetMovePointForAbility);
            mySystem.Unregister<SE_Ability.CanceMovePoint>(CanceMovePointForAbility);
            mySystem.Unregister<SE_Behaviour.Event_StopMove>(BehaviourStopMove);
            mySystem.Unregister<SE_Behaviour.Event_SetKeepEyesOnGPO>(SetKeepEyesOnGPO);
            mySystem.Unregister<SE_GPO.Event_KnockbackMovePoint>(SetKnockbackMovePoint);
            mySystem.Unregister<SE_GPO.Event_StrikeFlyMovePoint>(SetStrikeFlyMovePoint);
            mySystem.Unregister<SE_Entity.Event_IsShowEntity>(SetHideEntity);
            mySystem.Unregister<SE_Entity.SyncPointAndRota>(SetSyncMovePoint);
            mySystem.Unregister<SE_AI.Event_DriveState>(OnDriveStateCallBack);
            mySystem.Unregister<SE_GPO.Event_JumpTypeChange>(OnJumpCallBack);
            mySystem.Unregister<SE_GPO.Event_SlideMove>(OnSlideMoveCallBack);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
            mySystem.Unregister<SE_GPO.Event_PlayerDrive>(OnPlayerDriveCallBack);
            mySystem.Unregister<SE_GPO.UseWeapon>(OnUseWeaponCallBack);
            mySystem.Unregister<SE_Behaviour.Event_AfterBehaviorConfigInit>(AfterBehaviorConfigInitCallBack);
            mySystem.Unregister<SE_AbilityEffect.Event_UpdateEffect>(OnUpdateEffectCallBack);
            mySystem.Unregister<SE_Behaviour.Event_LookTarget>(OnLookTargetCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
            controller = null;
            groundCheckRaycastHit = null;
            aiSystem = null;
        }

        private void OnUpdate(float delta) {
            if (!hasBehaviorInit) {
                return;
            }

            if (ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver) {
                StopMove();
                RemoveUpdate(OnUpdate);
                return;
            }
            UpdateEnableNavMesh();
            UpdateRotate();
            Move(delta);
            CheckGroundedDeltaTime();
            CheckMovePoint();
        }


        public void OnUpdateEffectCallBack(ISystemMsg body, SE_AbilityEffect.Event_UpdateEffect env) {
            switch (env.Effect) {
                case AbilityEffectData.Effect.GpoMoveSpeedRate:
                    ability_SpeedRatio = env.Value;
                    break;
            }
        }

        private void GetCharacterController() {
            controller = entity.GetComponent<CharacterController>();
            if (controller == null) {
                Debug.LogError("缺少移动组件 CharacterController");
            }
        }

        private void GetNavMeshAgent() {
            navMeshAgent = entity.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null) {
                navMeshAgent.enabled = false;
                navMeshAgent.updatePosition = false;
                navMeshAgent.updateRotation = false;
            } else {
                Debug.LogError("缺少移动组件 NavMeshAgent");
            }
        }

        private void OnUseWeaponCallBack(ISystemMsg body, SE_GPO.UseWeapon ent) {
            useWeapon = ent.Weapon;
            gunMoveSpeed = 0f;
            if (useWeapon != null) {
                useWeapon.Dispatcher(new SE_Weapon.Event_GetGunMoveSpeed {
                    CallBack = value => {
                        gunMoveSpeed = value;
                    }
                });
            }
        }

        private void AfterBehaviorConfigInitCallBack(ISystemMsg body, SE_Behaviour.Event_AfterBehaviorConfigInit ent) {
            hasBehaviorInit = true;
        }

        /// <summary>
        /// 被驾驶状态改变
        /// </summary>
        /// <param name="ent"></param>
        private void OnDriveStateCallBack(ISystemMsg body, SE_AI.Event_DriveState ent) {
            driveState = ent.DriveGpoType;
            if (driveState == GPOData.GPOType.Role)
                if (navMeshAgent == null) {
                    return;
                }
            var isEnable = driveState != GPOData.GPOType.Role;
            controller.enabled = isEnable;
            if (isEnable == false) {
                StopMove();
            }
        }

        /// <summary>
        /// 驾驶员状态改变 ,  怪物作为驾驶员的情况下，停止自身的行为，转为由怪物驱动
        /// </summary>
        /// <param name="ent"></param>
        private void OnPlayerDriveCallBack(ISystemMsg body, SE_GPO.Event_PlayerDrive ent) {
            isPlayerDrive = ent.IsDrive;
            controller.enabled = !isPlayerDrive;
            if (isPlayerDrive) {
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
            if (isMovePoint == false || isPlayerDrive || driveState == GPOData.GPOType.Role || navMeshAgent == null) {
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

        private void OnSlideMoveCallBack(ISystemMsg body, SE_GPO.Event_SlideMove ent) {
            isSlide = ent.IsSlide;
            if (isSlide == false || driveState == GPOData.GPOType.Role || isPlayerDrive) {
                moveAddForce = Vector3.zero;
            } else {
                moveAddForce = ent.SlideVelocity;
                CancelJumpEndY();
                this.controller?.Move(ent.SlideVelocity);
            }
        }

        private void BehaviourStopMove(ISystemMsg body, SE_Behaviour.Event_StopMove ent) {
            if (isAbilityMove || isHideEntity || driveState == GPOData.GPOType.Role || isPlayerDrive) {
                return;
            }
            StopMove();
        }

        private void SetKeepEyesOnGPO(ISystemMsg body, SE_Behaviour.Event_SetKeepEyesOnGPO ent) {
            isKeepEyesOnGPO = ent.IsEnabled;
            gpoEyesOn = ent.TargetGPO;
        }

        private void StopMove() {
            if (isMovePoint == true) {
                Dispatcher(new SE_AI.Event_IsMovePoint {
                    IsTrue = false
                });
                Dispatcher(new SE_GPO.Event_MovePointEnd());
            }
            this.movePoint = iEntity.GetPoint();
            this.moveDir = Vector3.zero;
            Dispatcher(new SE_GPO.Event_MoveDir {
                MoveDir = moveDir
            });
            isMovePoint = false;
        }

        public void SetSyncMovePoint(ISystemMsg body, SE_Entity.SyncPointAndRota ent) {
            iEntity.SetPoint(ent.Point);
        }

        public void SetMovePointForAbility(ISystemMsg body, SE_Ability.MovePoint entData) {
            if (isHideEntity) {
                return;
            }
            isAbilityMove = true;
            isKnockbackMove = false;
            isStrikeFlyMove = false;
            abilityMoveSpeed = entData.moveSpeed;
            SetMovePoint(entData.movePoint, AIData.MoveType.Run);
        }

        private void JumpOverMove(ISystemMsg body, SE_AI.Event_JumpOverMove obj) {
            jumpOverLastTime = jumpOverObstacleDuration;
        }

        public void SetMovePointForAction(ISystemMsg body, SE_Behaviour.Event_MovePoint entData) {
            SetMovePoint(entData.movePoint, entData.MoveType);
        }

        public void SetKnockbackMovePoint(ISystemMsg body, SE_GPO.Event_KnockbackMovePoint entData) {
            if (isAbilityMove || isStrikeFlyMove || isHideEntity || isSlide || isPlayerDrive ||
                IsControllerEnabled() == false) {
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

        public void SetStrikeFlyMovePoint(ISystemMsg body, SE_GPO.Event_StrikeFlyMovePoint ent) {
            if (isAbilityMove || isHideEntity || isSlide || isPlayerDrive || IsControllerEnabled() == false) {
                isStrikeFlyMove = false;
                return;
            }
            if (ent.StrikeFlyMove == Vector3.zero) {
                isStrikeFlyMove = false;
            } else {
                isStrikeFlyMove = true;
                this.controller?.Move(ent.StrikeFlyMove);
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
            if (driveState == GPOData.GPOType.Role || isPlayerDrive) {
                return;
            }

            var filter = new NavMeshQueryFilter() {
                agentTypeID = navMeshAgent.agentTypeID,
                areaMask = NavMesh.AllAreas
            };
            if (NavMesh.SamplePosition(movePoint, out NavMeshHit hit, navMeshSampleTolerance, filter)) {
                movePoint = hit.position;
            }
            // navMeshAgent.enabled = false;
            this.movePoint = movePoint;
            isMovePoint = true;
            this.moveType = moveType;
            isNavMeshSetDestination = false;
            Dispatcher(new SE_AI.Event_IsMovePoint {
                IsTrue = true, MoveType = moveType
            });
        }

        private void Move(float deltaTime) {
            if (jumpOverLastTime > 0) {
                jumpOverLastTime -= deltaTime;
            }
            if (moveDirUpdateTime > 0) {
                moveDirUpdateTime -= deltaTime;
            }
            CheckLookAt();
            if ((!isJump && !isMovePoint && isGround) || isStrikeFlyMove || isKnockbackMove || isHideEntity || isSlide || IsControllerEnabled() == false) {
                return;
            }
            CheckMoveDir();
#if UNITY_EDITOR
            if (navMeshAgent.isOnNavMesh) {
                Debug.DrawLine(iEntity.GetPoint(), (iEntity.GetPoint() + moveDir * 10), Color.blue);
                Debug.DrawLine(iEntity.GetPoint(), (iEntity.GetPoint() + Vector3.up * 10), Color.yellow);
            } else {
                Debug.DrawLine(iEntity.GetPoint(), (iEntity.GetPoint() + Vector3.up * 10), Color.red);
            }
            Debug.DrawLine(iEntity.GetPoint(), movePoint, Color.green);
#endif
            if (navMeshAgent.isOnNavMesh == false) {
                DelayGetNavMeshPoint();
            }
            var useDir = Vector3.zero;
            if (isJump) {
                JumpMove();
            } else {
                FallMove(deltaTime, weight);
            }
            var speed = GetSpeed();
            var forward = iEntity.GetForward();
            if (this.moveType == AIData.MoveType.BackMove) {
                forward *= -1;
            } else {
                forward = moveDir;
            }
            useDir.x = forward.x * speed;
            useDir.z = forward.z * speed;
            useDir.y = moveDirY;
            navMeshAgent.speed = speed;
            this.controller.Move((useDir + moveAddForce) * deltaTime);
            navMeshAgent.nextPosition = iEntity.GetPoint();
            if (CheckReachMovePoint()) {
                StopMove();
            }
        }

        private void UpdateRotate() {
            if (!isLookTarget) {
                return;
            }

            var lookDir = lookTarget.GetPoint() - navMeshAgent.transform.position;
            if (lookDir.sqrMagnitude == 0) {
                return;
            }

            lookDir.y = 0;
            lookDir.Normalize();
            Quaternion rotation = Quaternion.LookRotation(lookDir);
            navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, rotation, Time.deltaTime * 10f);
        }

        private void DelayGetNavMeshPoint() {
            if (entity == null) {
                return;
            }

            if (Time.realtimeSinceStartup - delayGetNavMeshPointTime < 3f) {
                return;
            }
            delayGetNavMeshPointTime = Time.realtimeSinceStartup;
            if (navMeshAgent.isOnNavMesh) {
                return;
            }
            var navMeshPoint = GetNavMeshPoint(iEntity.GetPoint());
            iEntity.SetPoint(navMeshPoint);
            navMeshAgent.nextPosition = iEntity.GetPoint();
        }

        private Vector3 GetNavMeshPoint(Vector3 point) {
            var filter = new NavMeshQueryFilter() {
                agentTypeID = navMeshAgent.agentTypeID,
                areaMask = NavMesh.AllAreas
            };
            if (NavMesh.SamplePosition(point + 0.1f * Vector3.up, out NavMeshHit hit, 3, filter)) {
                point = hit.position;
            } else {
                Debug.Log($"{iGPO.GetName()} - {point} 3m 范围内找不到可寻路点");
            }
            return point;
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
            if (this.moveType != AIData.MoveType.BackMove) {
                var nowPoint = iEntity.GetPoint();
                nowPoint.y = movePoint.y;
                if (jumpOverLastTime <= 0 && movePoint != Vector3.zero) {
                    if (navMeshAgent.enabled == false || navMeshAgent.isOnNavMesh == false) {
                        moveDir = (movePoint - nowPoint).normalized;
                    } else {
                        if (isNavMeshSetDestination == false) {
                            navMeshAgent.SetDestination(this.movePoint);
                            isNavMeshSetDestination = true;
                        }

                        if (!navMeshAgent.pathPending) {
                            if (moveDirUpdateTime <= 0) {
                                moveDir = navMeshAgent.desiredVelocity.normalized;
                                moveDirUpdateTime = moveDirUpdateInterval;
                            }
                        }
                    }
                }
            } else {
                moveDir = -iEntity.GetForward();
            }

            Dispatcher(new SE_GPO.Event_MoveDir {
                MoveDir = moveDir
            });
        }

        private void CheckLookAt() {
            if (moveDir != Vector3.zero && !isKeepEyesOnGPO) {
                if (this.moveType == AIData.MoveType.BackMove) {
                    var targetRotation = Quaternion.LookRotation(-moveDir, Vector3.up);
                    iEntity.SetRota(targetRotation);
                } else {
                    var targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                    iEntity.SetRota(targetRotation);
                }
            }

            if (isKeepEyesOnGPO) {
                Vector3 targetPoint = gpoEyesOn.GetPoint();
                iEntity.LookAT(new Vector3(targetPoint.x, iEntity.GetPoint().y, targetPoint.z));
            }
        }

        private bool CheckReachMovePoint() {
            var distance = Vector3.Distance(iEntity.GetPoint(), movePoint);
            return distance <= reachDistance;
        }

        private float GetSpeed() {
            Dispatcher(new SE_AI.Event_GetActivateStatus() {
                CallBack = SetActivateStatus
            });
            var runSpeed = goldDashBehavior.PatrolWalkSpeed;
            if (isAbilityMove) {
                runSpeed = abilityMoveSpeed;
            } else if (isInFightStatus) {
                runSpeed = goldDashBehavior.FightRunSpeed;
            } else if (isInAlertStatus) {
                runSpeed = goldDashBehavior.AlertWalkSpeed;
            }
            runSpeed *= 1 + ability_SpeedRatio;
            return runSpeed;
        }

        private void FallMove(float deltaTime, float weight) {
            if (moveDirY >= 0) {
                moveDirY = 0f;
            }
            moveDirY -= gravity * weight * deltaTime;
            var maxDownGravitey = -weight * gravity;
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

        private void CheckGroundedDeltaTime() {
            if (checkGroundDeltaTime > 0f) {
                checkGroundDeltaTime -= Time.deltaTime;
                return;
            }
            checkGroundDeltaTime = 0.5f;
            CheckGrounded();
        }

        private void CheckGrounded() {
            if (this.controller.isGrounded) {
                IsGround(true, 0f);
                return;
            }
            var point = this.iEntity.GetPoint() + Vector3.up * 0.2f;
            var count = Physics.RaycastNonAlloc(point, Vector3.down, groundCheckRaycastHit, 3f);
            this.groundDis = -1f;
            GetGoundData(count, point, groundCheckRaycastHit);
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
            jumpEndPy = iEntity.GetPoint().y + envData.jumpHeight / gravity;
            this.jumpType = jumpType;
            isJump = CharacterData.IsJump(jumpType);
            if (isJump == false) {
                CancelJumpEndY();
            } else {
                startJumpTime = Time.time;
            }
        }

        private void OnSwitchAllBehaviorCallBack(SM_Sausage.SausageSwitchAllBehavior ent) {
            StopMove();
        }

        private void OnLookTargetCallBack(ISystemMsg body, SE_Behaviour.Event_LookTarget ent) {
            isLookTarget = ent.isLookTarget;
            lookTarget = ent.lookTarget;
        }

        private void CancelJumpEndY() {
            jumpEndPy = iEntity.GetPoint().y;
            moveDir.y = 0f;
            isJump = false;
        }

        private void SetActivateStatus(bool isInAlertStatus, bool isInFightStatus) {
            this.isInAlertStatus = isInAlertStatus;
            this.isInFightStatus = isInFightStatus;
        }
    }
}