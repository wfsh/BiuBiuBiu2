using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class MovePointRaycastHit : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public int IgnoreGpoID = 0;
            public int IgnoreTeamId = 0;
            public int LayerMask = 0;
            public GPOData.GPOType IgnoreGPOType;
            public Action<GameObject, RaycastHit> HitCallBack;
        }
        private Vector3 prevPoint = Vector3.zero;
        private RaycastHit[] raycastHit;
        private bool isPlay = false;
        private int ignoreGpoID = 0;
        private int ignoreTeamId = 0;
        private GPOData.GPOType ignoreGpoType = GPOData.GPOType.NULL;
        private int layerMask = 0;
        private Action<GameObject, RaycastHit> hitCallBack;
        private List<GameObject> ignoreGameObj;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.IgnoreGpoID, initData.HitCallBack);
            SetIgnoreTeamId(initData.IgnoreTeamId);
            SetIgnoreGPOType(initData.IgnoreGPOType);
            SetLayerMask(initData.LayerMask);
        }

        protected override void OnStart() {
            base.OnStart();
            ignoreGameObj = new List<GameObject>();
            AddUpdate(OnUpdate);
            StartCheck();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            raycastHit = null;
            this.hitCallBack = null;
            ignoreGameObj = null;
        }

        public void SetData(int ignoreID, Action<GameObject, RaycastHit> hitCallBack) {
            this.ignoreGpoID = ignoreID;
            this.hitCallBack = hitCallBack;
        }

        public void SetLayerMask(int layerMask) {
            this.layerMask = layerMask;
        }

        public void SetIgnoreTeamId(int teamId) {
            this.ignoreTeamId = teamId;
        }

        public void SetIgnoreGPOType(GPOData.GPOType ignoreGpoType) {
            this.ignoreGpoType = ignoreGpoType;
        }
        
        private void StartCheck() {
            prevPoint = iEntity.GetPoint();
            raycastHit = new RaycastHit[10];
            isPlay = true;
        }

        private void OnUpdate(float delta) {
            if (isPlay == false) {
                return;
            }
            var forward = iEntity.GetRota() * Vector3.forward;
            var distance = Vector3.Distance(iEntity.GetPoint(), prevPoint);
            var count = Raycast(prevPoint, forward, raycastHit, distance, this.layerMask);
            if (count > 0) {
                SendHitGPO(count, raycastHit, distance);
            }
            if (iEntity != null) {
                prevPoint = iEntity.GetPoint();
            }
        }

        virtual protected int Raycast(Vector3 point, Vector3 forward, RaycastHit[] raycastHit,  float distance, int layerMask) {
            return Physics.RaycastNonAlloc(point, forward, raycastHit, distance, layerMask);
        }

        public void SendHitGPO(int count, RaycastHit[] list, float maxDistance) {
            var distance = maxDistance;
            GameObject hitObj = null;
            var isHit = false;
            var hitRay = new RaycastHit();
            for (int i = 0; i < count; i++) {
                var ray = list[i];
                if (ray.collider == null) {
                    continue;
                }
                var gameObj = ray.collider.gameObject;
                if (IsIgnoreGameObj(gameObj)) {
                    continue;
                }
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
                    var gpo = hitEntity.GetGPO();
                    if (gpo != null) {
                        if (gpo.GetGPOType() == ignoreGpoType) {
                            continue;
                        }
                    }
                }
                if (distance > ray.distance) {
                    hitObj = gameObj;
                    hitRay = ray;
                    distance = ray.distance;
                    isHit = true;
                }
            }
            if (isHit == false) {
                return;
            }
            isPlay = false;
            OnHitGameObj(hitObj, hitRay);
            hitCallBack?.Invoke(hitObj, hitRay);
        }

        protected void SetIgnoreGameObj(GameObject ignoreGameObj) {
            this.ignoreGameObj.Add(ignoreGameObj);
        }

        // 检查是否有需要忽略的 GameObj
        protected bool IsIgnoreGameObj(GameObject gameObj) {
            return ignoreGameObj.Contains(gameObj);
        }

        virtual protected void OnHitGameObj(GameObject gameObj, RaycastHit hitRay) {
        }
    }
}