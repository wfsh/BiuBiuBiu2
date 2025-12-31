using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityDragonFullScreenAOE : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_DragonFullScreenAOESpawner Param;
            public int SpawnPointIndex;
        }
        
        private SAB_DragonFullScreenAOESystem abSystem;
        private AbilityM_DragonFullScreenAOESpawner param;
        private ServerGPO fireGPO;
        private Vector3 curPos;
        private Quaternion curRot;
        private Collider[] hitResultArr;
        private float curTime;
        private float checkAttackTimer;
        private Vector3 fightRangeCenter;
        private float fightRangeRadius;
        private bool isPlayAttackEffect;
        private int spawnPointIndex;
        private Vector3[] spawnPoints;
        private Dictionary<int, float> hited = new Dictionary<int, float>();

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_DragonFullScreenAOESystem)mySystem;
            var initData = (InitData)initDataBase;
            param = initData.Param;
            spawnPointIndex = initData.SpawnPointIndex;
            spawnPoints = param.M_SpawnPoints[spawnPointIndex];
            fireGPO = abSystem.FireGPO;
            hitResultArr = new Collider[20];
            curPos = fireGPO.GetPoint();
            curRot = fireGPO.GetRota();
        }

        protected override void OnStart() {
            base.OnStart();
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_PlaySkillEnd());
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FullScreenAOE());
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonAoe,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = param.M_LifeTime,
                }
            });
            foreach (var offset in spawnPoints) {
                PlayWarningEffect(offset);
            }

            PlayAttackEffect();
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData() {
                CallBack = SetFightRangeDataCallBack
            });

            AddUpdate(OnUpdate);
        }

        private void SetFightRangeDataCallBack(Vector3 center, float radius, float endTime, bool isInFightRange) {
            fightRangeCenter = center;
            fightRangeRadius = radius;
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
        }

        protected override void OnClear() {
            base.OnClear();
            spawnPoints = null;
            RemoveUpdate(OnUpdate);
            fireGPO = null;
            hitResultArr = null;
            abSystem = null;
            param = null;
            abSystem = null;
        }

        private void OnUpdate(float deltaTime) {
            if (fireGPO == null || fireGPO.IsClear()) {
                return;
            }

            curTime += deltaTime;
            if (curTime >= param.M_SpawnTime) {
                foreach (var point in spawnPoints) {
                    CheckDamage(point, curTime - param.M_SpawnTime, deltaTime);
                }
            }
        }

        private void PlayWarningEffect(Vector3 offset) {
            var center = curPos;
            var point = center + curRot * offset;
            var distance = Mathf.Min(param.M_MoveSpeed * param.M_LifeTime, param.M_MaxDistance);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_AOERectangleWarning),
                InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                    In_StartPoint = point,
                    In_StartRota = curRot,
                    In_StartScale = new Vector3(2 * param.M_Radius[spawnPointIndex], 1, distance),
                    In_LifeTime = param.M_SpawnTime,
                },
            });
        }

        private void PlayAttackEffect() {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                FireGPO = iGPO,
                MData = AbilityM_PlayEffect.CreateForID(abSystem.InData.In_PlayEffectId),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = curPos + new Vector3(0f, 0.2f, 0f),
                    In_StartRota = curRot,
                    In_LifeTime = param.M_LifeTime,
                },
            });
        }

        private void CheckDamage(Vector3 offset, float moveTime, float delta) {
            var center = curPos;
            var forward = curRot * Vector3.forward;
            var point = center + curRot * offset;
            var distance = Mathf.Min(param.M_MoveSpeed * moveTime, param.M_MaxDistance);
            var halfDis = distance / 2;
            if (checkAttackTimer < 0) {
                int length = Physics.OverlapBoxNonAlloc(point + forward * halfDis, new Vector3(param.M_Radius[spawnPointIndex], param.M_Radius[spawnPointIndex], halfDis), hitResultArr, curRot, LayerData.ServerLayerMask);
                for (int i = 0; i < length; i++) {
                    var hit = hitResultArr[i];
                    HitGameObj(hit.transform.gameObject);
                }

                checkAttackTimer += 0.1f;
            } else {
                checkAttackTimer -= delta;
            }

#if UNITY_EDITOR
            DrawAttackRange(point, param.M_Radius[spawnPointIndex], forward * distance, delta);
#endif
        }

        private void HitGameObj(GameObject gameObj) {
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null) {
                if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                    return;
                }

                var entity = hitType.MyEntity;
                if (entity == null || entity.GetGPO() == null) {
                    return;
                }

                var hitGpo = entity.GetGPO();
                var hitId = entity.GetGPOID();
                if (hitId == fireGPO.GetGpoID() || fireGPO.GetTeamID() == hitGpo.GetTeamID()) {
                    return;
                }

                if (hited.TryGetValue(hitId, out var value) && value >= Time.time) {
                    return;
                }

                hited[hitId] = Time.time + param.M_AttackCheckTime;
                Vector3 gpoPoint = hitGpo.GetPoint();
                gpoPoint.y = fightRangeCenter.y;
                float sqrtDisToOri = (gpoPoint - fightRangeCenter).sqrMagnitude;
                if (sqrtDisToOri >= Mathf.Pow(fightRangeRadius, 2)) {
                    return;
                }

                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = entity.GetGPO(),
                    isHead = false,
                    hitPoint = Vector3.zero,
                    SourceAbilityType = AbilityData.SAB_DragonFullScreenAOE,
                });
            }
        }

#if UNITY_EDITOR
        private void DrawAttackRange(Vector3 center, float radius, Vector3 dir, float dt) {
            Color rangeColor = Color.red;
            var rot = Quaternion.LookRotation(dir);
            var up = rot * Vector3.up * radius;
            var down = rot * Vector3.down * radius;
            var left = rot * Vector3.left * radius;
            var right = rot * Vector3.right * radius;
            Debug.DrawLine(center, center + up, rangeColor, dt);
            Debug.DrawLine(center, center + down, rangeColor, dt);
            Debug.DrawLine(center, center + left, rangeColor, dt);
            Debug.DrawLine(center, center + right, rangeColor, dt);
            Debug.DrawLine(center + dir, center + dir + up, rangeColor, dt);
            Debug.DrawLine(center + dir, center + dir + down, rangeColor, dt);
            Debug.DrawLine(center + dir, center + dir + left, rangeColor, dt);
            Debug.DrawLine(center + dir, center + dir + right, rangeColor, dt);
            Debug.DrawLine(center + up, center + dir + up, rangeColor, dt);
            Debug.DrawLine(center + down, center + dir + down, rangeColor, dt);
            Debug.DrawLine(center + left, center + dir + left, rangeColor, dt);
            Debug.DrawLine(center + right, center + dir + right, rangeColor, dt);
        }
#endif
    }
}