using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityBatEndAttack : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public Quaternion StartRota;
        }
        private S_Ability_Base abSystem;
        private Transform bodyTran;
        private EntityBase entity;
        private Quaternion startRota = Quaternion.identity;
        private Dictionary<int, ServerGPO> hitGPOData = new Dictionary<int, ServerGPO>();
        
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.StartRota);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            abSystem = (S_Ability_Base)mySystem;
            entity = (EntityBase)iEntity;
            entity.SetActive(false);
            AddCollier();
            GetTargetPoint();
            entity.SetActive(true);
        }

        private void AddCollier() {
            var collider = entity.GetComponentInChildren<Collider>();
            var colliderCheck = collider.gameObject.AddComponent<ColliderEnterCheck>();
            bodyTran = abSystem.FireGPO.GetBodyTran(GPOData.PartEnum.Body);
            colliderCheck.Init(OnHitCollider, null, null);
        }

        private void GetTargetPoint() {
            var forward = startRota * Vector3.forward;
            forward.Normalize();
            var targetPoint = bodyTran.position + forward * 1f;
            iEntity.SetPoint(targetPoint);
        }

        protected override void OnClear() {
            abSystem = null;
            bodyTran = null;
            entity = null;
            hitGPOData.Clear();
        }

        public void SetData(Quaternion rota) {
            startRota = rota;
        }

        private void OnHitCollider(Collider collision) {
            EnterGPO(collision.gameObject);
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
                if (hitGPOData.ContainsKey(gpo.GetGpoID())) {
                    return;
                }
                CheckGPOHurt((ServerGPO)gpo);
            }
        }

        private void CheckGPOHurt(ServerGPO gpo) {
            var tran = abSystem.FireGPO.GetBodyTran(GPOData.PartEnum.Head);
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
            hitGPOData.Add(gpo.GetGpoID(), gpo);
        }

        private Vector3 CalculatePoint(Vector3 startPoint, Vector3 endPoint) {
            var direction = endPoint - startPoint;
            direction.Normalize();
            var targetPoint = startPoint + direction * 1;
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