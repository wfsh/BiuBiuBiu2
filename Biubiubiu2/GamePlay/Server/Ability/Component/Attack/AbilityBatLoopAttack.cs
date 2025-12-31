using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityBatLoopAttack : ComponentBase {
        private S_Ability_Base abSystem;
        private float checkDeltaTime = 0.0f;
        private float hitDistance = 1f;
        private Collider collider;
        private Transform bodyTran;
        private List<IGPO> gpoList = new List<IGPO>();

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            var entity= (EntityBase)iEntity;
            abSystem = (S_Ability_Base)mySystem;
            collider = entity.GetComponentInChildren<Collider>();
            var colliderCheck = collider.gameObject.AddComponent<ColliderEnterCheck>();
            colliderCheck.Init(OnHitCollider, null, OnExitHitCollider);
            bodyTran = abSystem.FireGPO.GetBodyTran(GPOData.PartEnum.Body);
        }

        protected override void OnClear() {
            abSystem = null;
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            iEntity.SetPoint(bodyTran.position);
            if (checkDeltaTime > 0) {
                checkDeltaTime -= Time.deltaTime;
                return;
            }
            checkDeltaTime = 0.2f;
            CheckGPOHurt();
        }

        private void OnHitCollider(Collider collision) {
            EnterGPO(collision.gameObject);
        }

        private void OnExitHitCollider(Collider collision) {
            ExitGPO(collision.gameObject);
        }

        private void EnterGPO(GameObject gameObj) {
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null) {
                if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                    return;
                }
                if (hitType.MyEntity == null || hitType.MyEntity.GetGPO() == null) {
                    return;
                }
                var gpo = hitType.MyEntity.GetGPO();
                if (gpo.GetGpoID() == abSystem.FireGPO.GetGpoID()) {
                    return;
                }
                for (int i = 0; i < gpoList.Count; i++) {
                    var saveGPO = gpoList[i];
                    if (saveGPO.GetGpoID() == gpo.GetGpoID()) {
                        return;
                    }
                }
                gpoList.Add(gpo);
            }
        }

        private void ExitGPO(GameObject gameObj) {
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null) {
                if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                    return;
                }
                if (hitType.MyEntity == null || hitType.MyEntity.GetGPO() == null) {
                    return;
                }
                var gpo = hitType.MyEntity.GetGPO();
                for (int i = 0; i < gpoList.Count; i++) {
                    var saveGPO = gpoList[i];
                    if (saveGPO.GetGpoID() == gpo.GetGpoID()) {
                        gpoList.RemoveAt(i);
                    }
                }
            }
        }

        private void CheckGPOHurt() {
            var tran = abSystem.FireGPO.GetBodyTran(GPOData.PartEnum.Head);
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.IsClear()) {
                    continue;
                }
                var body = gpo.GetBodyTran(GPOData.PartEnum.Body);
                var targetPoint = gpo.GetPoint();
                if (body != null) {
                    targetPoint = body.position;
                }
                var hitPoint = CalculatePoint(tran.position, targetPoint);
                PlayHitEffect(hitPoint);
                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = gpo,
                    hitPoint = hitPoint
                });
            }
        }

        private Vector3 CalculatePoint(Vector3 startPoint, Vector3 endPoint) {
            var direction = endPoint - startPoint;
            direction.Normalize();
            var targetPoint = startPoint + direction * hitDistance;
            return targetPoint;
        }

        private void PlayHitEffect(Vector3 hitPoint) {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                FireGPO = iGPO,
                MData = AbilityM_PlayEffect.CreateForID(AbilityM_PlayEffect.ID_AttackHit),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = hitPoint,
                    In_StartRota = Quaternion.identity
                },
            });
        }
    }
}