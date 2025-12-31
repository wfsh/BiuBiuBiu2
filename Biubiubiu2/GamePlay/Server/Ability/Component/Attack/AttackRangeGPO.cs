using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AttackRangeGPO : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public Vector3 CheckPoint;
            public float Range;
            public bool IsSelfHurt;
            public float MaxHurtRatio;
            public bool IsRayRange;
        }
        private S_Ability_Base abSystem;
        private Vector3 checkPoint;
        private float range;
        public Color rangeColor = Color.red;
        private float drawDuration = 1.0f; // 持续时间
        private RaycastHit[] raycastHit;
        private Dictionary<int, IGPO> hitGPOData = new Dictionary<int, IGPO>();
        private List<Vector3> checkPoints = new List<Vector3>();
        private bool isSelfHurt = true;
        private float MaxHurtRatio = 1f;
        private Func<IGPO, float, float> getHurtRatioFunc;
        private bool isRayRange = true;

        protected override void OnAwake() {
            Register<SE_Ability.Ability_InitGetHurtRatioFunc>(OnInitGetHurtRatioFunc);
            var initData = (InitData)initDataBase;
            GetRangeGPOList(initData.CheckPoint, initData.Range);
            SetSelfHurt(initData.IsSelfHurt);
            SetMaxDistanceHurtRatio(initData.MaxHurtRatio);
            SetRangeType(initData.IsRayRange);
        }

        protected override void OnStart() {
            base.OnStart();
            abSystem = (S_Ability_Base)mySystem;
            raycastHit = new RaycastHit[10];
            mySystem.AddUpdate(OnUpdate);
            if (isRayRange) {
                CheckRayHit();
            } else {
                CheckRange();
            }
        }

        protected override void OnClear() {
            abSystem = null;
            mySystem.RemoveUpdate(OnUpdate);
            hitGPOData.Clear();
            Unregister<SE_Ability.Ability_InitGetHurtRatioFunc>(OnInitGetHurtRatioFunc);
        }

        private void OnUpdate(float deltaTime) {
#if UNITY_EDITOR
            DrawAttackRange(this.checkPoint, this.range);
            for (int i = 0; i < checkPoints.Count; i++) {
                Debug.DrawLine(iEntity.GetPoint(), checkPoints[i], Color.green, drawDuration);
            }
#endif
        }
        
        public void SetSelfHurt(bool isSelfHurt) {
            this.isSelfHurt = isSelfHurt;
        }
        
        public void SetMaxDistanceHurtRatio(float hurtRatio) {
            this.MaxHurtRatio = hurtRatio;
        }
        
        public void SetRangeType(bool isRayRange) {
            this.isRayRange = isRayRange;
        }

        void DrawAttackRange(Vector3 center, float radius) {
            Debug.DrawLine(center, center + Vector3.forward * radius, rangeColor, drawDuration);
            Debug.DrawLine(center, center + Vector3.back * radius, rangeColor, drawDuration);
            Debug.DrawLine(center, center + Vector3.left * radius, rangeColor, drawDuration);
            Debug.DrawLine(center, center + Vector3.right * radius, rangeColor, drawDuration);
            // 你可以添加更多的线条来更精确地表示一个圆
            var segments = 10;
            var angle = 0f;
            for (int i = 0; i < segments; i++) {
                var startPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                angle += 2 * Mathf.PI / segments;
                var endPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Debug.DrawLine(startPoint, endPoint, rangeColor, drawDuration);
            }
        }

        public void GetRangeGPOList(Vector3 checkPoint, float range) {
            if (range <= 0) {
                Debug.Log("GetRangeGPO 范围小于 0");
                return;
            }
            this.checkPoint = checkPoint;
            this.range = range;
        }
                
        // 检查和 GPO 之间是否有障碍物
        private void CheckRayHit() {
            var gpoList = abSystem.GPOList;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (this.isSelfHurt == false) {
                    if (gpo.GetTeamID() == abSystem.FireGPO.GetTeamID()) {
                        continue;
                    }
                }
                var distance = (checkPoint - gpo.GetPoint()).magnitude;
                if (distance > Mathf.Max(range * 2f, 10f)) {
                    continue;
                }
                var hitTypes = gpo.GetAllCanHitPart();
                for (int j = 0; j < hitTypes.Count; j++) {
                    var hitType = hitTypes[j];
                    var hitTran = hitType.GetTransform();
                    var checkDistance = (this.checkPoint - hitTran.position).magnitude;
                    if (CheckRaycast(hitTran, checkDistance, gpo)) {
                        HitGPO(gpo, checkDistance);
                        break;
                    }
                }
            }
        }

        private void CheckRange() {
            var gpoList = abSystem.GPOList;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (this.isSelfHurt == false) {
                    if (gpo.GetTeamID() == abSystem.FireGPO.GetTeamID()) {
                        continue;
                    }
                }
                var distance = (checkPoint - gpo.GetPoint()).magnitude;
                if (distance <= range) {
                    HitGPO(gpo, distance);
                }
            }
        }

        private bool CheckRaycast(Transform hitTran, float distance, IGPO checkGpo) {
            var forward = (hitTran.position - iEntity.GetPoint()).normalized;
            var startPoint = iEntity.GetPoint() - forward * 0.1f;
            var isHit = false;
            var count = Physics.RaycastNonAlloc(startPoint, forward, raycastHit, distance, LayerData.ServerLayerMask | LayerData.DefaultLayerMask);
            if (count > 0) {
                isHit = SendHitGPO(count, raycastHit, checkGpo);
            }
            return isHit;
        }
        
        public bool SendHitGPO(int count, RaycastHit[] list, IGPO checkGpo) {
            var isHit = false;
            var hitDistance = float.MaxValue;
            for (int i = 0; i < count; i++) {
                var ray = list[i];
                if (ray.collider == null) {
                    continue;
                }
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                var hitGpoId = 0;
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        continue;
                    }
                    if (hitType.MyEntity != null){
                        hitGpoId = hitType.MyEntity.GetGPO().GetGpoID();
                    }
                }
#if UNITY_EDITOR
                Debug.DrawLine(iEntity.GetPoint(), ray.point, Color.magenta, 1f);
#endif
                var checkDistance = Vector3.Distance(iEntity.GetPoint(), ray.point);
                if (checkDistance < hitDistance) {
                    hitDistance = checkDistance;
                    if (checkDistance < range) {
                        if (hitGpoId == checkGpo.GetGpoID()) {
                            isHit = true;
                        } else {
                            isHit = false;
                        }
                    }else{
                        isHit = false;
                    }
                }
            }
            return isHit;
        }

        private void HitGPO(IGPO hitGpo, float hitDistance) {
            if (hitGPOData.ContainsKey(hitGpo.GetGpoID()) || hitGpo.IsDead() || hitGpo.IsGodMode()) {
                return;
            }
            hitGPOData.Add(hitGpo.GetGpoID(), hitGpo);
            var hurtRatio = GetHurtRatio(hitGpo, hitDistance);
            mySystem.Dispatcher(new SE_Ability.HitGPO {
                hitGPO = hitGpo,
                hitPoint = hitGpo.GetPoint(),
                HurtRatio = hurtRatio,
            });
        }

        private float GetHurtRatio(IGPO hitGpo, float hitDistance) {
            float hurtRatio;
            if (getHurtRatioFunc != null) {
                hurtRatio = getHurtRatioFunc(hitGpo, hitDistance);
            } else {
                var distanceRatio = Mathf.Min(1f, Mathf.Max(0f, hitDistance - 1f) / (range - 1f));
                hurtRatio = 1f - distanceRatio * (1f - MaxHurtRatio);
            }
            return hurtRatio;
        }

        private void OnInitGetHurtRatioFunc(ISystemMsg body, SE_Ability.Ability_InitGetHurtRatioFunc ent) {
            getHurtRatioFunc = ent.getHurtRatioFunc;
        }
    }
}