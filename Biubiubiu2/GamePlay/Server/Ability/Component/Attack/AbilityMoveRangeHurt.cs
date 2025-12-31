using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityMoveRangeHurt : ComponentBase {
        private SAB_MoveRangeHurtSystem abSystem;
        private AbilityIn_MoveRangeHurt inData;
        private Vector3 point;
        private ServerGPO fireGPO;
        private Collider[] cols;
        private Vector3 fightRangeCenter;
        private float fightRangeRadius;
        private bool blockDamage;
        private Dictionary<int, float> hited = new Dictionary<int, float>();

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_MoveRangeHurtSystem)mySystem;
            inData = (AbilityIn_MoveRangeHurt)abSystem.InData;
            fireGPO = abSystem.FireGPO;
            point = inData.In_StartPoint;
        }

        protected override void OnStart() {
            base.OnStart();
            cols = new Collider[10];
            AddUpdate(OnUpdate);
            
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData() {
                CallBack = SetFightRangeDataCallBack
            });
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            fireGPO = null;
            base.OnClear();
        }

        private void OnUpdate(float deltaTime) {
            if (fireGPO == null || fireGPO.IsClear() || fireGPO.IsDead()) {
                return;
            }

            point += inData.In_StartDir * inData.In_MoveSpeed * deltaTime;
            CheckDamage(point, inData.In_Rangle);
#if UNITY_EDITOR
            DrawSphere(point, inData.In_Rangle);
#endif

            if (Vector3.SqrMagnitude(point - inData.In_StartPoint) > inData.In_MaxDistance * inData.In_MaxDistance) {
                MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                    AbilityId = abSystem.GetAbilityId(),
                });
            }
        }
        
        private void SetFightRangeDataCallBack(Vector3 center, float radius, float endTime, bool blockDamage) {
            fightRangeCenter = center;
            fightRangeRadius = radius;
            this.blockDamage = blockDamage;
        }

        private bool IsBlockDamage(IGPO gpo) {
            if (!blockDamage) {
                return false;
            }

            Vector3 gpoPoint = gpo.GetPoint();
            gpoPoint.y = fightRangeCenter.y;
            float sqrtDisToOri = (gpoPoint - fightRangeCenter).sqrMagnitude;
            if (sqrtDisToOri >= fightRangeRadius * fightRangeRadius) {
                return true;
            }

            return false;
        }

        private void CheckDamage(Vector3 point, float radius) {
            var count = Physics.OverlapSphereNonAlloc(point, radius, cols, LayerData.ServerLayerMask | LayerData.DefaultLayerMask | LayerData.TerrainLayerMask);
            var isHit = false;
            for (int i = 0; i < count; i++) {
                var col = cols[i];
                if (col == null) {
                    continue;
                }

                var gameObj = col.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        continue;
                    }

                    var hitEntity = hitType.MyEntity;
                    if (hitEntity != null) {
                        if (hitEntity.GetGPOID() == fireGPO.GetGpoID()) {
                            continue;
                        }

                        if (hitEntity.GetTeamID() == fireGPO.GetTeamID()) {
                            continue;
                        }

                        if (IsBlockDamage(hitEntity.GetGPO())) {
                            continue;
                        }

                        if (hited.TryGetValue(hitEntity.GetGPOID(), out var time) && Time.time < time) {
                            continue;
                        }

                        hited[hitEntity.GetGPOID()] = Time.time + inData.In_ATK;
                        mySystem.Dispatcher(new SE_Ability.HitGPO {
                            hitGPO = hitType.MyEntity.GetGPO(),
                            isHead = hitType.Part == GPOData.PartEnum.Head,
                            HurtRatio = 1f,
                        });
                    }
                }
            }
        }

        private void DrawSphere(Vector3 point, float radius, int deltaDegree = 5) {
            var preXZ = Vector3.zero;
            var preXY = Vector3.zero;
            var preYZ = Vector3.zero;
            var startDraw = false;
            for (int i = 0; i <= 360; i += deltaDegree) {
                var rad = i * Mathf.Deg2Rad;
                var xz = point + new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
                var xy = point + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
                var yz = point + new Vector3(0, Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
                if (startDraw) {
                    Debug.DrawLine(preXZ, xz, Color.red);
                    Debug.DrawLine(preXY, xy, Color.red);
                    Debug.DrawLine(preYZ, yz, Color.red);
                }

                startDraw = true;
                preXZ = xz;
                preXY = xy;
                preYZ = yz;
            }
        }
    }
}