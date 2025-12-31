using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 巡逻点组件
    public class ServerAIPatrolPoint : ComponentBase {
        public SceneConfig.PatrolType patrolType = SceneConfig.PatrolType.Range; // 巡逻类型
        public List<Vector3> pointList = new List<Vector3>(); // 巡逻点列表
        private Vector3 startPoint; // 起始点
        public float radius; // 半径
        public float area; // 半径
        private int currentPointIndex = 0;
        private Vector3 nextPoint;
        private float delayChangeNextPointTime = 0.0f;
        private NavMeshAgent navMeshAgent;
        private bool isMovePoint = false;
        public float checkInterval = 1f; // 检测间隔
        public int checkStuckIndex = 1; // 检测间隔
        private const float STUCK_CHECK_INTERVAL = 0.5f;
        public float stuckThreshold = 0.2f; // 判定卡住的范围（单位：米）
        private Vector3 lastPosition; // 上一次的位置

        protected override void OnAwake() {
            mySystem.Register<SE_AI.Event_SetPatrolPoint>(OnSetPatrolPointCallBack);
            mySystem.Register<SE_Behaviour.Event_GetNextPatrolPoint>(OnGetNextPatrolPointCallBack);
            mySystem.Register<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            pointList = null;
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_AI.Event_SetPatrolPoint>(OnSetPatrolPointCallBack);
            mySystem.Unregister<SE_Behaviour.Event_GetNextPatrolPoint>(OnGetNextPatrolPointCallBack);
            mySystem.Unregister<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            startPoint = iEntity.GetPoint();
            currentPointIndex = 0;
            navMeshAgent = GetNavMeshAgent();
            if (navMeshAgent == null) {
                Debug.LogError($"{iGPO.GetName()} - GetNavMeshAgent() == null");
            } else {
                var rangePoint = new Vector3(startPoint.x + Random.Range(-area, area), startPoint.y,
                    startPoint.z + Random.Range(-area, area));
                var point = GetNavMeshPoint(rangePoint);
                if (point != Vector3.zero) {
                    mySystem.Dispatcher(new SE_Entity.SyncPointAndRota {
                        Point = point,
                        Rota = iEntity.GetRota(),
                    });
                }
                navMeshAgent.nextPosition = iEntity.GetPoint();
            }
            nextPoint = GeneratePatrolPoint();
        }

        private NavMeshAgent GetNavMeshAgent() {
            var entity = (EntityBase)iEntity;
            return entity.GetComponent<NavMeshAgent>();
        }

        private void OnUpdate(float delta) {
            if (ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver) {
                return;
            }
            UpdatePatrolPoint();
            CheckStuck();
        }

        private void UpdatePatrolPoint() {
            if (patrolType == SceneConfig.PatrolType.Range) {
                return;
            }
            if (pointList.Count == 0) {
                return;
            }
            CheckNextPoint();
            var previousPoint = startPoint;
            for (int i = 0; i < pointList.Count; i++) {
                Debug.DrawLine(previousPoint, pointList[i], Color.magenta);
                previousPoint = pointList[i];
            }
        }

        private void CheckNextPoint() {
            var distance = Vector3.Distance(nextPoint, iEntity.GetPoint());
            if (distance < 1f) {
                currentPointIndex++;
                if (currentPointIndex >= pointList.Count) {
                    currentPointIndex = 0;
                }
                nextPoint = GeneratePatrolPoint();
            }
        }

        private void OnGetNextPatrolPointCallBack(ISystemMsg body, SE_Behaviour.Event_GetNextPatrolPoint ent) {
            if (Time.realtimeSinceStartup - delayChangeNextPointTime < 2f) {
                ent.CallBack(nextPoint);
                return;
            }
            delayChangeNextPointTime = Time.realtimeSinceStartup;
            nextPoint = GeneratePatrolPoint();
            ent.CallBack(nextPoint);
        }

        private void OnSetPatrolPointCallBack(ISystemMsg body, SE_AI.Event_SetPatrolPoint ent) {
            var data = ent.PatrolPointData;
            patrolType = data.PatrolType;
            pointList = data.PointList;
            radius = data.Radius * 0.5f;
            area = ent.Area * 0.5f;
        }

        // 根据pointList 和 radius 生成巡逻点
        private Vector3 GeneratePatrolPoint() {
            // 生成巡逻点
            var point = startPoint;
            var rangePoint = Vector3.zero;
            if (patrolType == SceneConfig.PatrolType.Point) {
                point = pointList[currentPointIndex];
                rangePoint = new Vector3(point.x + Random.Range(-radius, radius), point.y,
                    point.z + Random.Range(-radius, radius));
            } else {
                rangePoint = new Vector3(startPoint.x + Random.Range(-area, area), startPoint.y,
                    startPoint.z + Random.Range(-area, area));
            }
            var checkPoint = GetNavMeshPoint(rangePoint);
            if (checkPoint != Vector3.zero) {
                return checkPoint;
            }
            return startPoint;
        }

        private Vector3 GetNavMeshPoint(Vector3 point) {
            var filter = new NavMeshQueryFilter() {
                agentTypeID = navMeshAgent.agentTypeID, areaMask = NavMesh.AllAreas
            };
            if (NavMesh.SamplePosition(point, out NavMeshHit hit, 10f, filter)) {
                point = hit.position;
            } else {
                Debug.Log($"{iGPO.GetName()} - {point}  range: 10m 范围内找不到可寻路点");
            }
            return point;
        }

        private void OnIsMovePointCallBack(ISystemMsg body, SE_AI.Event_IsMovePoint ent) {
            isMovePoint = ent.IsTrue;
        }

        private void CheckStuck() {
            if (checkInterval > 0f) {
                checkInterval -= Time.deltaTime;
                return;
            }
            checkInterval = 1f;
            if (isMovePoint == false || navMeshAgent == null || !navMeshAgent.enabled) {
                checkStuckIndex = 0;
                return;
            }
            var distanceMoved = Vector3.Distance(iEntity.GetPoint(), lastPosition);
            if (distanceMoved < stuckThreshold) {
                checkStuckIndex++;
                if (checkStuckIndex >= 10) {
                    checkStuckIndex = 0;
                    ResetNavMeshAgent();
                    // 重置位置
                    mySystem.Dispatcher(new SE_Entity.SyncPointAndRota {
                        Point = startPoint,
                        Rota = iEntity.GetRota(),
                    });
                    navMeshAgent.nextPosition = iEntity.GetPoint();
                }
            } else {
                checkStuckIndex = 0;
            }
            lastPosition = iEntity.GetPoint();
        }

        private void ResetNavMeshAgent() {
            navMeshAgent.enabled = false;
            navMeshAgent.enabled = true;
        }
    }
}