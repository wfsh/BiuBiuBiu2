using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityPlayRay : ServerNetworkComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public bool IsFollowFireGPO;
        }
        
        private AbilityIn_PlayRay useInData;
        private ServerGPO fireGPO;
        private RaycastHit[] raycastHit;
        private int ignoreGpoID;
        private int ignoreTeamId;
        private float attackTimer;
        private RaycastHit currentHit;
        private bool isFollowFireGPO = false;

        protected override void OnAwake() {
            base.OnAwake();
            AwakeInit();
        }

        private void AwakeInit() {
            var abSystem = (SAB_PlayRaySystem)mySystem;
            useInData = (AbilityIn_PlayRay)abSystem.InData;
            fireGPO = abSystem.FireGPO;
            var initData = ((InitData)initDataBase);
            isFollowFireGPO = initData.IsFollowFireGPO;
        }

        protected override void OnStart() {
            base.OnStart();
            ignoreGpoID = fireGPO.GpoID;
            ignoreTeamId = fireGPO.GetTeamID();
            raycastHit = new RaycastHit[10];
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }

        private void OnUpdate(float deltaTime) {
            if (fireGPO == null || fireGPO.IsClear() || fireGPO.IsDead()) {
                return;
            }
            if (isFollowFireGPO) {
                iEntity.SetPoint(fireGPO.GetPoint());
                iEntity.SetRota(fireGPO.GetRota());
            }
            if (attackTimer > 0) {
                attackTimer -= deltaTime;
                return;
            }
            if (Raycast()) {
                OnHitGameObj(currentHit.collider.gameObject, currentHit);
            }
            attackTimer = useInData.In_RayATKInterval;
        }

        private bool Raycast() {
            if (useInData.In_RayATK <= 0) {
                return false;
            }
            var distance = (float)useInData.In_MaxDistance;
            var count = Physics.RaycastNonAlloc(iEntity.GetPoint(), iEntity.GetForward(), raycastHit, distance, LayerData.ServerLayerMask | LayerData.DefaultLayerMask);
#if UNITY_EDITOR
            Debug.DrawRay(iEntity.GetPoint(), iEntity.GetForward() * distance, Color.red);
#endif
            var isHit = false;
            var hitRay = new RaycastHit();
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null) {
                    continue;
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
                    hitRay = ray;
                    distance = ray.distance;
                    isHit = true;
                }
            }

            if (!isHit) {
                currentHit = default;
                return false;
            }

            currentHit = hitRay;
            return true;
        }

        private void OnHitGameObj(GameObject gameObj, RaycastHit hitRay) {
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null && hitType.MyEntity != null && hitType.MyEntity.GetGPO() != null) {
                var checkHitRatio = Random.Range(0f, 1f);
                var hitRatio = 1f;
                fireGPO.Dispatcher(new SE_GPO.Event_GetHitRatio {
                    HitGpoType = hitType.MyEntity.GetGPO().GetGPOType(),
                    CallBack = (ratio) => { hitRatio = ratio; }
                });
                if (checkHitRatio > hitRatio) {
                    return;
                }

                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = hitType.MyEntity.GetGPO(),
                    isHead = hitType.Part == GPOData.PartEnum.Head,
                    hitPoint = hitRay.point,
                    HurtRatio = 1f,
                });
            }
        }
    }
}
