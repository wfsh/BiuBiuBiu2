using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 直升机移动
    public class ServerHelicopterMoveForCharacter : ServerAIMove {
        private bool isNavMeshSetDestination = false;
        private S_AI_Base aiSystem;
        private GPOM_Helicopter useMData;
        private Transform rootBody; // 根节点 用于控制上下移动
        private AIData.MoveType moveType = AIData.MoveType.Walk; // 移动类型 Walk, Run, BackMove, Stand
        private GPOData.GPOType driveState = GPOData.GPOType.NULL;
        private Vector3 moveDir = Vector3.zero;
        private Vector3 movePoint = Vector3.zero;
        private Vector3 prevPoint = Vector3.zero;
        private float movePointProtectTime = 0f;
        private float protectDeltaTime = 0.0f;
        private float checkInterval = 1f; // 检测间隔
        private float stuckThreshold = 1f; // 判定卡住的范围（单位：米）
        private bool isMovePoint = false;
        private bool isHideEntity = false;
        private bool isDead = false;
        private bool isPlayerDrive = false;
        private Vector3 lastPosition; // 上一次的位置
        private const float gravitey = 1f; // 重力
        private const float weight = 30f; // 重量
        private float groundDis = 0f;
        private float checkGroundDeltaTime = 0.0f;
        private bool isGround = false;
        private float moveDirY = 0f;
        private float syncTime = 0.1f;
        private float runSpeed = 0f; // 步行速度
        private float runRotationSpeed = 0f;

        // === 直升机高度控制 ===
        private float raycastDistance = 10f; // 用于检测障碍物
        private float deltaGetHeight = 0f;
        private float targetHeight = 0f; // 目标飞行高度

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Behaviour.Event_MovePoint>(SetMovePointForAction); // 行为树设置目标移动点
            mySystem.Register<SE_Behaviour.Event_StopMove>(BehaviourStopMove); // 行为树停止移动
            mySystem.Register<SE_Entity.Event_IsShowEntity>(SetHideEntity); // 设置是否显示实体
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnDeadCallBack); // 设置是否死亡
            mySystem.Register<SE_AI.Event_DriveState>(OnDriveStateCallBack); // 设置驾驶状态
            mySystem.Register<SE_GPO.Event_PlayerDrive>(OnPlayerDriveCallBack); // 设置玩家驾驶状态
        }

        protected override void OnStart() {
            base.OnStart();
            aiSystem = (S_AI_Base)mySystem;
            useMData = (GPOM_Helicopter)aiSystem.MData;
            this.runSpeed = useMData.MoveSpeed;
            this.runRotationSpeed = useMData.RotaSpeed;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            rootBody = entity.GetBodyTran(GPOData.PartEnum.RootBody);
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Unregister<SE_Behaviour.Event_StopMove>(BehaviourStopMove);
            mySystem.Unregister<SE_Entity.Event_IsShowEntity>(SetHideEntity);
            mySystem.Unregister<SE_AI.Event_DriveState>(OnDriveStateCallBack);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
            mySystem.Unregister<SE_GPO.Event_PlayerDrive>(OnPlayerDriveCallBack);
            controller = null;
            rootBody = null;
        }

        private void OnUpdate(float delta) {
            if (ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver) {
                StopMove();
                RemoveUpdate(OnUpdate);
                return;
            }
            CheckStuck();
            UpdateEnableNavMesh();
            Move(delta);
            CheckMovePoint();
            CheckGrounded();
        }
        
        /// <summary>
        /// 怪物状态 - 如，坦克，飞机
        /// </summary>
        /// <param name="ent"></param>
        private void OnDriveStateCallBack(ISystemMsg body, SE_AI.Event_DriveState ent) {
            driveState = ent.DriveGpoType;
            var isEnable = driveState != GPOData.GPOType.Role;
            controller.enabled = isEnable;
            if (isEnable == false) {
                StopMove();
            }
        }

        /// <summary>
        /// 驾驶员状态改变 - 如：AI，玩家
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

        private void CheckStuck() {
            if (checkInterval > 0f) {
                checkInterval -= Time.deltaTime;
                return;
            }
            checkInterval = 1.5f;
            if (navMeshAgent == null || !navMeshAgent.enabled) return;
            var distanceMoved = Vector3.Distance(iEntity.GetPoint(), lastPosition);
            if (distanceMoved < stuckThreshold) {
                Debug.Log($"ResetNavMeshAgent Name: {iGPO.GetName()}");
                ResetNavMeshAgent();
            }
            lastPosition = iEntity.GetPoint();
        }

        private void ResetNavMeshAgent() {
            navMeshAgent.enabled = false;
            navMeshAgent.enabled = true;
        }

        private void BehaviourStopMove(ISystemMsg body, SE_Behaviour.Event_StopMove ent) {
            if (isHideEntity || driveState == GPOData.GPOType.Role || isPlayerDrive) {
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

        public void SetMovePointForAction(ISystemMsg body, SE_Behaviour.Event_MovePoint entData) {
            SetMovePoint(entData.movePoint, entData.MoveType);
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
            this.movePoint = movePoint;
            isMovePoint = true;
            moveDir = (movePoint - iEntity.GetPoint()).normalized;
            this.moveType = moveType; // 如果传过来的是 BackMove 就是朝向上仍然朝向目标，但是会向着自身和目标相反的方向移动
            isNavMeshSetDestination = false;
            Dispatcher(new SE_AI.Event_IsMovePoint {
                IsTrue = true, MoveType = moveType
            });
        }

        private void Move(float deltaTime) {
            if (isMovePoint == false || isHideEntity || IsControllerEnabled() == false) {
                return;
            }
            CheckMoveDir();
#if UNITY_EDITOR
            if (navMeshAgent.isOnNavMesh) {
                Debug.DrawLine(iEntity.GetPoint(), (iEntity.GetPoint() + moveDir * 10), Color.yellow);
            }
            Debug.DrawLine(iEntity.GetPoint(), movePoint, Color.red);
#endif
            AdjustHeight(deltaTime);
            MoveHorizontal(deltaTime);
        }

        /// <summary>
        /// 水平移动
        /// </summary>
        private void MoveHorizontal(float deltaTime) {
            FallMove(deltaTime, weight);
            var useDir = Vector3.zero;
            var forward = iEntity.GetForward();
            if (this.moveType == AIData.MoveType.BackMove) {
                forward *= -1;
            }
            useDir.x = forward.x * runSpeed;
            useDir.z = forward.z * runSpeed;
            useDir.y = moveDirY;
            navMeshAgent.speed = runSpeed;
            ControllerMove(useDir * deltaTime);
            navMeshAgent.nextPosition = iEntity.GetPoint();
            var distance = Vector3.Distance(iEntity.GetPoint(), movePoint);
            if (distance <= 0.5f) {
                StopMove();
            }
        }

        private void CheckMoveDir() {
            if (!navMeshAgent.enabled || !navMeshAgent.isOnNavMesh) {
                moveDir = (movePoint - iEntity.GetPoint()).normalized;
            } else {
                if (!isNavMeshSetDestination) {
                    navMeshAgent.SetDestination(this.movePoint);
                    isNavMeshSetDestination = true;
                }
                moveDir = navMeshAgent.desiredVelocity.normalized;
            }
            Dispatcher(new SE_GPO.Event_MoveDir {
                MoveDir = moveDir
            });
            if (moveDir != Vector3.zero) {
                moveDir.y = 0;
                var targetRotation =
                    Quaternion.LookRotation(this.moveType == AIData.MoveType.BackMove ? -moveDir : moveDir,
                        Vector3.up);
                var distance = (50f - (movePoint - iEntity.GetPoint()).magnitude) * 0.1f;
                if (distance < 1) {
                    distance = 1;
                }
                // **平滑插值旋转**
                var newRotation = Quaternion.Slerp(iEntity.GetRota(), targetRotation, distance * Time.deltaTime);
                // **更新旋转**
                iEntity.SetRota(newRotation);
            }
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
        }

        /// <summary>
        /// 调整飞行高度
        /// </summary>
        private void AdjustHeight(float deltaTime) {
            var rootPointY = rootBody.localPosition.y;
            if (Time.realtimeSinceStartup - deltaGetHeight > 0.1f) {
                deltaGetHeight = Time.realtimeSinceStartup;
                targetHeight = GetTargetHeight(rootPointY);
            }
            var currentHeight = Mathf.Lerp(rootPointY, targetHeight, useMData.HeightAdjustSpeed * deltaTime);
            if (Mathf.Abs(rootPointY - currentHeight) > 0.001f) {
                rootBody.localPosition = new Vector3(0f, currentHeight, 0f);
                SyncUpperBodyRota();
            }
        }
        
        private float GetTargetHeight(float currentHeight) {
            var limitMinHeight = 0f;
            var limitMaxHeight = GetMaxHeight();
            var targetHeight = currentHeight;
            // **检测前方是 5 米否有障碍物， 有的话提前开始执行降落逻辑**
            if (CheckFrontBlocked() || CheckFrontBlocked2()) {
                // **如果前方有障碍物，降低飞行高度**
                targetHeight = limitMinHeight;
            } else if (CheckBottomBlocked() == false) {
                // **如果头顶没有障碍物，恢复飞行高度**
                targetHeight = limitMaxHeight;
            }
            if (targetHeight < limitMinHeight) {
                targetHeight = limitMinHeight;
            }
            if (targetHeight > limitMaxHeight) {
                targetHeight = limitMaxHeight;
            }
            return targetHeight;
        }
        
        // 检测前方是否有障碍物
        private bool CheckFrontBlocked() {
            var frontBlocked = CheckRayHit(rootBody.position + Vector3.up * 1f, rootBody.forward, raycastDistance, 0);
            return frontBlocked;
        }
        // 检测前方是否有障碍物
        private bool CheckFrontBlocked2() {
            var frontBlocked2 = CheckRayHit(rootBody.position + Vector3.up * 2f, rootBody.forward, raycastDistance, 0);
            return frontBlocked2;
        }
        // 检测前方头顶是否有障碍物
        private bool CheckBottomBlocked() {
            var bottomBlocked = CheckRayHit(rootBody.position + Vector3.up * 3f, rootBody.forward, raycastDistance, 1);
            return bottomBlocked;
        }
        
        private void SyncUpperBodyRota() {
            if (Time.realtimeSinceStartup - syncTime < 0.1f) {
                return;
            }
            syncTime = Time.realtimeSinceStartup;
            Rpc(new Proto_AI.Rpc_SyncHelicopterRootPosY {
                py = targetHeight,
            });
        }

        private bool CheckRayHit(Vector3 startPoint, Vector3 dir, float distance, int index) {
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, startPoint + dir * distance, index == 0 ? Color.cyan : Color.green);
#endif
            var isHit = false;
            var count = Physics.RaycastNonAlloc(startPoint, dir, raycastHit, distance, ~LayerData.ClientLayerMask);
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        continue;
                    }
                    var hitEntity = hitType.MyEntity;
                    if (hitEntity != null && hitEntity.GetGPO() != null) {
                        continue;
                    }
                }
                return true;
            }
            return false;
        }
        
        private float GetMaxHeight() {
            var startPoint = iEntity.GetPoint();
            var targetPy = 0f;
            var distance = useMData.MaxFlyHeight;
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, startPoint + Vector3.up * distance, Color.red);
#endif
            var count = Physics.RaycastNonAlloc(startPoint, Vector3.up, raycastHit, distance, ~LayerData.ClientLayerMask);
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        continue;
                    }
                    var hitEntity = hitType.MyEntity;
                    if (hitEntity != null && hitEntity.GetGPO() != null) {
                        continue;
                    }
                }
                if (distance > ray.distance) {
                    distance = ray.distance;
                }
            }
            return distance - 3f;
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
    }
}