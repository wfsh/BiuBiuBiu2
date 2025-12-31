using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIFindInsightTarget : ServerNetworkComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float CheckDistance;
            public int LayerMask;
            public int IgnoreTeamId ;
            public bool IgnoreCollierTrigger;
        }
        private S_AI_Base aiSystem;
        private float checkDistance = 30.0f;
        private AIEntity mAIEntity;
        private float delayCheckTargetTime = 0;
        private IGPO targetGPO;
        
        //射线检测相关
        private RaycastHit[] raycastHit;
        private int layerMask = ~LayerData.ClientLayerMask;
        private int ignoreTeamId = 0;
        private bool ignoreCollierTrigger = true;
        
        protected override void OnAwake() {
            aiSystem = (S_AI_Base)mySystem;
            mySystem.Register<SE_AI.Event_GetInsightTarget>(OnEvent_GetInsightTarget);
            var initData = (InitData)initDataBase;
            SetData(initData.CheckDistance, initData.LayerMask, initData.IgnoreTeamId, initData.IgnoreCollierTrigger);
        }

        protected override void OnStart() {
            base.OnStart();
            raycastHit = new RaycastHit[10];
            ignoreTeamId = iGPO.GetTeamID();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            mAIEntity = (AIEntity)iEntity;
        }

        private void OnUpdate(float deltaTime) {
            if (delayCheckTargetTime < 0) {
                var tGPO = GetTarget();
                if (targetGPO != tGPO) {
                    targetGPO = tGPO;
                    mySystem.Dispatcher(new SE_AI.Event_SetInsightTarget() {
                        TargetGPO = GetTarget()
                    });
                }
                
                delayCheckTargetTime = 0.5f;
            } else {
                delayCheckTargetTime -= deltaTime;
            }
        }
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_AI.Event_GetInsightTarget>(OnEvent_GetInsightTarget);
        }
        public void SetData(float checkDistance,int layerMask,int teamId,bool isTrue) {
            this.checkDistance = checkDistance;
            this.layerMask = layerMask;
            this.ignoreTeamId = teamId;
            this.ignoreCollierTrigger = isTrue;
        }

        private void OnEvent_GetInsightTarget(ISystemMsg body, SE_AI.Event_GetInsightTarget ent) {
            ent.CallBack.Invoke(GetTarget());
        }

        private IGPO GetTarget() {
            if (isSetEntityObj == false) {
                return null;
            }
            return FindNearestGPOInFront();
        }
        
         /// <summary>
        /// 查找最近的目标对象 GPO
        /// </summary>
        private IGPO FindNearestGPOInFront() {
            var gpoList = aiSystem.GPOList;
            IGPO nearestGpo = null;
            var nearestValue = checkDistance;
            foreach (var gpo in gpoList) {
                if (gpo.IsClear() || ignoreTeamId == gpo.GetTeamID() || gpo.IsGodMode()) {
                    continue;
                }
                var directionToPet = gpo.GetPoint() - mAIEntity.GetPoint();
                var distanceToPet = directionToPet.magnitude;
                if (distanceToPet <= nearestValue) {
                    var isObstacle = CheckObstacle(aiSystem.GetGPO(), gpo);
                    if (isObstacle == false) {
                        nearestValue = distanceToPet;
                        nearestGpo = gpo;
                    }
                }
            }
            return nearestGpo;
        }
        
        // 检查和 GPO 之间是否有障碍物
        private bool CheckObstacle(IGPO targetGpo, IGPO useGpo) {
            var useTran = CheckGPOTran(useGpo);
            var targetTran = CheckGPOTran(targetGpo);
            if (useTran == null || targetTran == null) {
                return false;
            }
            var startPoint = useTran.position;
            var endPoint = targetTran.position;
            var forward = (endPoint - startPoint).normalized;
            var distance = Vector3.Distance(startPoint, endPoint);
            var count = Physics.RaycastNonAlloc(startPoint, forward, raycastHit, distance,layerMask);
            var isHit = false;
            if (count > 0) {
                isHit = CheckObstacleRaycastHit(count);
            }
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, endPoint, isHit ? Color.red : Color.green);
#endif
            return isHit;
        }
        
        private bool CheckObstacleRaycastHit(int count) {
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    continue;
                }
                return true;
            }
            return false;
        }
        private Transform CheckGPOTran(IGPO igpo) {
            var tran = igpo.GetBodyTran(GPOData.PartEnum.Head);
            if (tran == null) {
                tran = igpo.GetBodyTran(GPOData.PartEnum.Body);
            }
            return tran;
        }
    }
}

