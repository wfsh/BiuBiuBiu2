using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityExpandBoom : ComponentBase {
        private SAB_ExpandBoomSystem abSystem;
        private AbilityIn_ExpandBoom useInData;
        private ServerGPO fireGPO;
        private float distance;
        private float timer;
        private float checkTimer;
        private List<IGPO> gpoList;
        private HashSet<IGPO> hited = new HashSet<IGPO>();
        
        private Vector3 fightRangeCenter;
        private float fightRangeRadius;
        private bool blockDamage;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_ExpandBoomSystem)mySystem;
            useInData = abSystem.useInData;
            fireGPO = abSystem.FireGPO;
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = (gpos => gpoList = gpos)
            });
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData() {
                CallBack = SetFightRangeDataCallBack
            });
            if (useInData.In_ExpandSpeed == 0) {
                distance = useInData.In_MaxDistance;
                CheckDamage();
            }
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }

        private void OnUpdate(float delta) {
#if UNITY_EDITOR
            DrawLine();
#endif
            if (useInData.In_ExpandSpeed == 0 ||
                distance >= useInData.In_MaxDistance) {
                return;
            }
            
            if (checkTimer > 0) {
                checkTimer -= delta;
            } else {
                distance += useInData.In_ExpandSpeed * delta;
                checkTimer = 0.01f;
                CheckDamage();
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

        private void CheckDamage() {
            for (int i = gpoList.Count - 1; i >= 0; i--) {
                var gpo = gpoList[i];

                if (hited.Contains(gpo)) {
                    continue;
                }

                if (gpo.GetTeamID() == fireGPO.GetTeamID()) {
                    continue;
                }

                if (gpo.IsDead() || gpo.IsClear()) {
                    continue;
                }

                if (IsBlockDamage(gpo)) {
                    return;
                }

                var targetDir = gpo.GetPoint() - useInData.In_StartPoint;
                targetDir.y = 0;

                float dis = Vector3.SqrMagnitude(targetDir);
                if (dis > distance * distance) {
                    continue;
                }

                if (Mathf.Abs(useInData.In_StartPoint.y - gpo.GetPoint().y) > useInData.In_CheckHight) {
                    continue;
                }

                if (useInData.In_CheckBlock && PhysicsUtil.CheckBlocked(
                        useInData.In_StartPoint,
                        gpo.GetBodyTran(GPOData.PartEnum.Body).position,
                        IgnoreFunc,
                        LayerData.DefaultLayerMask | LayerData.TerrainLayerMask,
                        out var hitPoint)) {
                    continue;
                }

                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = gpo,
                    isHead = false,
                    hitPoint = gpo.GetPoint(),
                    HurtRatio = 1f,
                });

                hited.Add(gpo);
            }
        }

        
        private bool IgnoreFunc(RaycastHit hit) {
            var hitType = hit.collider.gameObject.GetComponent<HitType>();
            if (hitType != null) {
                if (hitType.Layer == GPOData.LayerEnum.Ignore || (hitType.MyEntity != null && hitType.MyEntity.GetTeamID() == fireGPO.GetTeamID())) {
                    return true;
                }
            }
            return false;
        }

        private void DrawLine() {
            DrowCircle(useInData.In_StartPoint, Vector3.forward, distance, 360);
        }

        private void DrowCircle(Vector3 start, Vector3 dir, float dis, float angle) {
            angle = Mathf.Min(angle, 360);
            int delta = 5;
            dir *= dis;
            var prevEnd1 = start + dir;
            var prevEnd2 = start + dir;
            for (int i = 0; i < angle / 2; i += delta) {
                var end1 = start + Quaternion.Euler(0, i, 0) * dir;
                var end2 = start + Quaternion.Euler(0, -i, 0) * dir;
                Debug.DrawLine(prevEnd1, end1, Color.red);
                Debug.DrawLine(prevEnd2, end2, Color.red);
                prevEnd1 = end1;
                prevEnd2 = end2;
            }
        }
    }
}
