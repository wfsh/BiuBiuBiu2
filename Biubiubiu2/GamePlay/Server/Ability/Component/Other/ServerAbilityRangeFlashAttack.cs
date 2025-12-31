using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityRangeFlashAttack : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public bool IsSelfHurt; // 是否伤害自己
            public int IngoreGPOId; // 忽略的 GPO ID
            public Vector3 CheckPoint; // 检测点
            public float Range; // 范围
        }
        //选择器变量
        public struct HitGPOData {
            public IGPO HitGPO;
            public float Distance;
        }

        private int ingoreGPOId = 0;
        private bool isSelfHurt = true;
        private float range;
        private float MaxHurtRatio = 1f;
        private float drawDuration = 1.0f;
        private Vector3 checkPoint;

        private List<HitGPOData> hitGPOList = new List<HitGPOData>();
        private List<Vector3> checkPoints = new List<Vector3>();
        private Dictionary<int, IGPO> hitGPOData = new Dictionary<int, IGPO>();

        private Color rangeColor = Color.red;
        private S_Ability_Base abSystem;
        private RaycastHit[] raycastHit;
        
        //延迟触发伤害变量
        private bool isCheck = false;
        private float delayCheckTime = 1f;
        private AbilityData.PlayAbility_RangeFlash inData;
        

        protected override void OnAwake() {
            base.OnAwake();
            raycastHit = new RaycastHit[10];
            abSystem = (SAB_RangeFlashSystem)mySystem;
            inData = (AbilityData.PlayAbility_RangeFlash)abSystem.MData;
            delayCheckTime = inData.In_DelayCheckTime;
            var initData = (InitData)initDataBase;
            SetData(initData.IsSelfHurt, initData.IngoreGPOId, initData.CheckPoint, initData.Range);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            Check();
            RPCFlashEffect();
        }
        
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            hitGPOData.Clear();
        }

        private void OnUpdate(float deltaTime) {
            if (isCheck) {
                return;
            }
#if UNITY_EDITOR
            DrawAttackRange(this.checkPoint, this.range);
            for (int i = 0; i < checkPoints.Count; i++) {
                Debug.DrawLine(iEntity.GetPoint(), checkPoints[i], Color.green, drawDuration);
            }
#endif
            delayCheckTime -= deltaTime;
            if (delayCheckTime <= 0) {
                isCheck = true;
                OnHitAllGPO();
            }
        }
        
        //选择器逻辑
        public void SetData(bool isSelfHurt,int igoreGPOId,Vector3 checkPoint, float range) {
            this.isSelfHurt = isSelfHurt;
            this.ingoreGPOId = igoreGPOId;
            if (range <= 0) {
                Debug.Log("GetRangeGPO 范围小于 0");
                return;
            }
            this.checkPoint = checkPoint;
            this.range = range;
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

        
        // 检查和 GPO 之间是否有障碍物
        private bool CheckObstacle(IGPO gpo, Vector3 usePosition) {
            var hitTypes = gpo.GetAllCanHitPart();
            if (hitTypes.Count <= 0) {
                return false;
            }
            int hitCount = 0;
            for (int j = 0; j < hitTypes.Count; j++) {
                var hitType = hitTypes[j];
                var hitTran = hitType.GetTransform();
                var startPoint = usePosition;
                var endPoint = hitTran.position;
                var forward = (endPoint - startPoint).normalized;
                var distance = Vector3.Distance(startPoint, endPoint);
                var count = Physics.RaycastNonAlloc(startPoint, forward, raycastHit, distance,LayerData.ServerLayerMask | LayerData.DefaultLayerMask);
                bool isObstacle = false;
                if (count > 0) {
                    isObstacle = CheckObstacleRaycastHit(count);
                }
                if (isObstacle) {
                    hitCount += 1;
                }
#if UNITY_EDITOR
                Debug.DrawLine(startPoint, endPoint, isObstacle ? Color.red : Color.green);
#endif
            }

            bool isHit = hitCount >= hitTypes.Count;
            return isHit;
        }
        
        public bool CheckObstacleRaycastHit(int count) {
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
        
        private void Check() {
            var gpoList = abSystem.GPOList;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.GetGpoID() == ingoreGPOId) {
                    continue;
                }

                if (this.isSelfHurt == false) {
                    if (gpo.GetTeamID() == abSystem.FireGPO.GetTeamID() || gpo.IsGodMode()) {
                        continue;
                    }
                }

                // 使用平方距离避免昂贵的开方运算
                var distance = (checkPoint - gpo.GetPoint()).magnitude;
                if (distance > range * 1.2f) {
                    continue;
                }
                bool isObstacle = CheckObstacle(gpo, checkPoint);
                if (isObstacle == false) {
                    var hitDistance = (this.checkPoint - gpo.GetPoint()).magnitude;
                    hitGPOList.Add(new HitGPOData() {
                        Distance = hitDistance,
                        HitGPO = gpo,
                    });
                }
            }
        }

        private void OnHitAllGPO() {
            for (int i = 0; i < hitGPOList.Count; i++) {
                HitGPOData hitGPOData = hitGPOList[i];
                HitGPO(hitGPOData.HitGPO, hitGPOData.Distance);
            }
        }

        private void HitGPO(IGPO hitGpo, float hitDistance) {
            if (hitGPOData.ContainsKey(hitGpo.GetGpoID())) {
                return;
            }

            hitGPOData.Add(hitGpo.GetGpoID(), hitGpo);
            var distanceRatio = Mathf.Min(1f, Mathf.Max(0f, hitDistance - 1f) / (range - 1f));
            var hurtRatio = 1f - (distanceRatio * (1f - MaxHurtRatio));
            mySystem.Dispatcher(new SE_Ability.HitGPO {
                hitGPO = hitGpo,
                hitPoint = hitGpo.GetPoint(),
                HurtRatio = hurtRatio,
            });
        }
        

        private void RPCFlashEffect() {
            int gpoCount = hitGPOList.Count;
            if (gpoCount > 0) {
                int[] gpoId = new int[gpoCount + 1];
                bool isGetOriginHitGPO = true;
                MsgRegister.Dispatcher(new SM_GPO.GetGPO {
                    GpoId = inData.In_IngoreGPOId,
                    CallBack = gpo => {
                        if (gpo != null) {
                            gpoId[gpoId.Length - 1] = gpo.GetGpoID();
                        } else {
                            isGetOriginHitGPO = false;
                        }
                    }
                });
                if (isGetOriginHitGPO == false) {
                    return;
                }
                for (int i = 0; i < gpoId.Length - 1; i++) {
                    gpoId[i] = hitGPOList[i].HitGPO.GetGpoID();
                }
                mySystem.Dispatcher(new SE_Ability.RPCAbility {
                    ProtoData = new Proto_Ability.Rpc_RangeFlash {
                        abilityModId = inData.ConfigId,
                        lifeTime = (ushort)Mathf.CeilToInt(inData.In_DelayCheckTime * 10f * 4),
                        GPOId = gpoId
                    }
                });
            }
        }
    }
}