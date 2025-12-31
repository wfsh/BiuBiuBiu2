using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SphereRaycastHit : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public int IgnoreTeamId;
            public int IgnoreGpoID;
            public float Range;
            public float Radius;
            public Action<GameObject, RaycastHit> HitCallBack;
        }
        // private Vector3 prevPoint = Vector3.zero;
        private Vector3 checkPoint = Vector3.zero;
        private float maxDistance = 0f;
        private float radius = 1f;
        private RaycastHit[] raycastHit = new RaycastHit[10];
        private bool isPlay = false;
        private int ignoreGpoID = 0;
        private int ignoreTeamId = 0;
        private int layerMask = 0;
        private Action<GameObject, RaycastHit> hitCallBack;
        private Action<float> rangeChangeCallBack;
        private List<GameObject> ignoreGameObj;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Ability.Ability_SetRangeChangeCallBack>(OnSetRangeChangeCallBack);
            mySystem.Register<SE_Ability.Ability_RaycastStartCheck>(OnStartCheckCallBack);
            var initData = (InitData)initDataBase;
            SetIgnoreTeamId(initData.IgnoreTeamId);
            SetData(initData.IgnoreGpoID, initData.HitCallBack, initData.Range, initData.Radius);
        }

        protected override void OnStart() {
            base.OnStart();
            ignoreGameObj = new List<GameObject>();
            AddUpdate(OnUpdate);
            // StartCheck();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Ability.Ability_SetRangeChangeCallBack>(OnSetRangeChangeCallBack);
            mySystem.Unregister<SE_Ability.Ability_RaycastStartCheck>(OnStartCheckCallBack);
            RemoveUpdate(OnUpdate);
            raycastHit = null;
            this.hitCallBack = null;
            this.rangeChangeCallBack = null;
            ignoreGameObj = null;
        }

        private void OnSetRangeChangeCallBack(ISystemMsg body, SE_Ability.Ability_SetRangeChangeCallBack ent) {
            this.rangeChangeCallBack = ent.Callback;
        }
        public void SetData(int ignoreID, Action<GameObject, RaycastHit> hitCallBack, float range, float radius) {
            this.ignoreGpoID = ignoreID;
            this.hitCallBack = hitCallBack;
            maxDistance = range;
            this.radius = radius;
        }

        public void SetLayerMask(int layerMask) {
            this.layerMask = layerMask;
        }

        public void SetIgnoreTeamId(int teamId) {
            this.ignoreTeamId = teamId;
        }

        public void OnStartCheckCallBack(ISystemMsg body, SE_Ability.Ability_RaycastStartCheck ent) {
            checkPoint = iEntity.GetPoint();
            Cast();
        }

        public void StartCheck(float distance) {
            maxDistance = distance;
            checkPoint = iEntity.GetPoint();
            raycastHit = new RaycastHit[10];
            isPlay = true;
        }

        public void InvokeRangeChangeCallback(float range) {
            this.rangeChangeCallBack?.Invoke(range);
        }

        private void OnUpdate(float delta) {
            if (isPlay == false) {
                return;
            }

            Cast();
        }

        private void Cast() {
            var forward = iEntity.GetRota() * Vector3.forward;
            // var distance = Vector3.Distance(iEntity.GetPoint(), prevPoint);
            var count = Physics.SphereCastNonAlloc(checkPoint, radius, forward, raycastHit, maxDistance,
                this.layerMask);
            if (count > 0) {
                SendHitGPO(count, raycastHit, maxDistance);
            }else {
                InvokeRangeChangeCallback(maxDistance);
            }

            if (iEntity != null) {
                checkPoint = iEntity.GetPoint();
            }
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

            var isHitGameObj = OnHitGameObj(hitObj, hitRay);
            if (isHitGameObj) {
                hitCallBack?.Invoke(hitObj, hitRay);
            }
        }

        protected void SetIgnoreGameObj(GameObject ignoreGameObj) {
            this.ignoreGameObj.Add(ignoreGameObj);
        }

        // 检查是否有需要忽略的 GameObj
        protected bool IsIgnoreGameObj(GameObject gameObj) {
            return ignoreGameObj.Contains(gameObj);
        }

        virtual protected bool OnHitGameObj(GameObject gameObj, RaycastHit hitRay) {
            return true;
        }
    }
}