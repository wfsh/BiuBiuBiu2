using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    
    [TaskName("开火范围区间移动")]
    [TaskCategory("AI")]
    public class ActionMoveForTargetRangeByTemplate : ActionComponent{
        [SerializeField]
        private float checkTime;
        [SerializeField]
        private float maxAxisAngle = 0f;
        [SerializeField]
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private float countCheckTime;
        private IGPO target;
        private RaycastHit[] raycastHit;
        private float minDistance = 0f;
        private float maxDistance = 0f;

        public override void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            raycastHit = new RaycastHit[10];
            var monsterBehaviour = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(iGPO.GetSign());
            minDistance = monsterBehaviour.CatchFireRange - 5;
            maxDistance = monsterBehaviour.CatchFireRange;
            if (minDistance < 0) {
                minDistance = 0;
            }
        }

        protected override void OnClear() {
            base.OnClear();
            iGPO.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            this.target = null;
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            this.target = ent.TargetGPO;
        }

        public override TaskStatus OnUpdate() {
            if (Time.realtimeSinceStartup - countCheckTime <= this.checkTime) {
                return TaskStatus.Running;
            }
            countCheckTime = Time.realtimeSinceStartup;
            if (this.target == null || this.target.IsClear()) {
                this.target = null;
                return TaskStatus.Running;
            }
            iGPO.Dispatcher(new SE_Behaviour.Event_MovePoint {
                movePoint = CalculateRandomPoint(target),
                MoveType = moveType
            });
            return TaskStatus.Running;
        }

        // 获取目标范围内的随机点
        public Vector3 CalculateRandomPoint(IGPO targetGpo) {
            Vector3 targetPoint = targetGpo.GetPoint();
            float angle = Random.Range(-0.5f * maxAxisAngle, 0.5f * maxAxisAngle) + 90;
            Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle));
            float distance = Random.Range(minDistance, maxDistance);
            Vector3 randomDiff = direction * distance;
            Vector3 axisForward = iGPO.GetPoint() - targetPoint;
            axisForward.y = 0;
            axisForward = axisForward.normalized;
            Vector3 axisRight = Vector3.Cross(Vector3.up, axisForward).normalized;
            randomDiff = randomDiff.x * axisRight + randomDiff.z * axisForward;
            Vector3 randomPoint = targetPoint + randomDiff;

            // 检查阻挡，寻找内侧可行点
            Vector3 targetHighestPoint = CheckGPOHighestPoint(targetGpo);
            Vector3 forward = (randomPoint - targetHighestPoint).normalized;
            float disToRandomPoint = Vector3.Distance(targetHighestPoint, randomPoint);
            int count = Physics.RaycastNonAlloc(targetHighestPoint, forward, raycastHit, disToRandomPoint, ~(LayerData.ClientLayerMask));
            if (count > 0) {
                bool hasPoint = CheckObstacleRaycastClosestHit(count, out Vector3 closestPoint);
                if (hasPoint) {
                    randomPoint = closestPoint;
                }
            }

            return randomPoint;
        }

        private Vector3 CheckGPOHighestPoint(IGPO iGpo) {
            var tran = iGpo.GetBodyTran(GPOData.PartEnum.Head);
            if (tran == null) {
                tran = iGpo.GetBodyTran(GPOData.PartEnum.Body);
            }
            if (tran == null) {
                return iEntity.GetPoint();
            }
            return tran.position;
        }

        private bool CheckObstacleRaycastClosestHit(int count, out Vector3 closestPoint) {
            bool hasPoint = false;
            float minDis = float.MaxValue;
            closestPoint = Vector3.zero;

            for (int i = 0; i < count; i++) {
                var hit = raycastHit[i];
                if (hit.collider == null || hit.collider.isTrigger) {
                    continue;
                }
                var gameObj = hit.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    continue;
                }

                if (hit.distance < minDis) {
                    closestPoint = hit.point;
                    hasPoint = true;
                }
            }

            return hasPoint;
        }
    }
}