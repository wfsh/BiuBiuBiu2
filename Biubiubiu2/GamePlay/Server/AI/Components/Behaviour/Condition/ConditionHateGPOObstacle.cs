using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("检查和仇恨目标直接是否有障碍物")]
    [TaskCategory("AI")]
    public class ConditionHateGPOObstacle : ConditionComponent {
        private IGPO target;

        public override void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            iGPO.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            target = null;
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            target = ent.TargetGPO;
        }

        public override TaskStatus OnUpdate() {
            if (target == null || target.IsClear()|| iEntity.IsClear()) {
                return TaskStatus.Failure;
            }
            
            var useTran = CheckGpoTran(iGPO);
            var targetTran = CheckGpoTran(target);
            if (useTran == null || targetTran == null) {
                return TaskStatus.Failure;
            }

            var startPoint = useTran.position;
            var endPoint = targetTran.position;
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, endPoint, Color.blue, 1f);
#endif
            if (PhysicsUtil.CheckBlocked(startPoint, endPoint, IgnoreFunc, LayerData.ServerLayerMask | LayerData.DefaultLayerMask, out Vector3 point)) {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
        
        private Transform CheckGpoTran(IGPO igpo) {
            var tran = igpo.GetBodyTran(GPOData.PartEnum.Head);
            if (tran == null) {
                tran = igpo.GetBodyTran(GPOData.PartEnum.Body);
            }
            return tran;
        }

        private bool IgnoreFunc(RaycastHit hit) {
            var hitType = hit.collider.gameObject.GetComponent<HitType>();
            if (hitType != null) {
                // 忽略空气墙与角色
                if (hitType.Layer == GPOData.LayerEnum.Ignore ||
                    hitType.MyEntity != null ||
                    hitType.MyEntity == iEntity) {
                    return true;
                }
            }

            return false;
        }
    }
}