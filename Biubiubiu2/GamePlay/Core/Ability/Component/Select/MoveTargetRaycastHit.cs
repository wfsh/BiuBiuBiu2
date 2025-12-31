using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class MoveTargetRaycastHit : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public int IgnoreGpoID;
            public int IgnoreTeamId;
            public int LayerMask;
            public bool IsIgnoreCollierTrigge;
            public float MaxDistance;
            public Action<bool, Vector3, RaycastHit> HitCallBack;
        }
        
        private RaycastHit[] raycastHit;
        private int ignoreGpoID = 0;
        private int ignoreTeamId = 0;
        private bool ignoreCollierTrigger = true;
        private int layerMask = ~LayerData.ClientLayerMask;
        private Action<bool, Vector3, RaycastHit> hitCallBack;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetIgnoreGpoID(initData.IgnoreGpoID);
            SetLayerMask(initData.LayerMask);
            SetIgnoreTeamId(initData.IgnoreTeamId);
            SetIgnoreCollierTrigger(initData.IsIgnoreCollierTrigge);
            StartCheck(initData.MaxDistance, initData.HitCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            raycastHit = null;
            this.hitCallBack = null;
        }

        public void SetIgnoreGpoID(int ignoreID) {
            this.ignoreGpoID = ignoreID;
        }

        public void SetLayerMask(int layerMask) {
            this.layerMask = layerMask;
        }

        public void SetIgnoreTeamId(int teamId) {
            this.ignoreTeamId = teamId;
        }

        public void SetIgnoreCollierTrigger(bool isTrue) {
            this.ignoreCollierTrigger = isTrue;
        }

        public void StartCheck(float maxDistance, Action<bool, Vector3, RaycastHit> hitCallBack) {
            if (hitCallBack == null) {
                return;
            }
            this.hitCallBack = hitCallBack;
            raycastHit = new RaycastHit[10];
            var forward = iEntity.GetRota() * Vector3.forward;
            var maxEndPoint = iEntity.GetPoint() + forward * maxDistance;
            PerfAnalyzerAgent.BeginSample("MoveTargetRaycastHit:RaycastNonAlloc");
            var count = Physics.RaycastNonAlloc(iEntity.GetPoint(), forward, raycastHit, maxDistance, this.layerMask);
            PerfAnalyzerAgent.EndSample("MoveTargetRaycastHit:RaycastNonAlloc");
            if (count > 0) {
                PerfAnalyzerAgent.BeginSample("MoveTargetRaycastHit:StartCheck");
                SendHitGPO(count, raycastHit, maxDistance, maxEndPoint);
                PerfAnalyzerAgent.EndSample("MoveTargetRaycastHit:StartCheck");
            } else {
                hitCallBack.Invoke(false, maxEndPoint, new RaycastHit());
            }
        }

        public void SendHitGPO(int count, RaycastHit[] list, float maxDistance, Vector3 maxEndPoint) {
            var distance = maxDistance;
            var isHit = false;
            var hitRay = new RaycastHit();
            var hitPoint = maxEndPoint;
            for (int i = 0; i < count; i++) {
                var ray = list[i];
                if (ray.collider == null) {
                    continue;
                }
                if (ignoreCollierTrigger == false) {
                    if (ray.collider.isTrigger) {
                        continue;
                    }
                }
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        continue;
                    }
                    var hitEntity = hitType.MyEntity;
                    if (hitEntity != null) {
                        if (hitEntity.GetGPOID() == ignoreGpoID) {
                            continue;
                        }
                        if (hitEntity.GetTeamID() == ignoreTeamId) {
                            continue;
                        }
                    }
                }
                if (distance > ray.distance) {
                    hitPoint = ray.point;
                    hitRay = ray;
                    distance = ray.distance;
                    isHit = true;
                }
            }
            hitCallBack?.Invoke(isHit, hitPoint, hitRay);
        }
    }
}