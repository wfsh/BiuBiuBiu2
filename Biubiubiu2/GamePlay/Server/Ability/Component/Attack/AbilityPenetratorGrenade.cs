using System;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Component;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityPenetratorGrenade : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public int IgnoreGpoID;
            public float MoveSpeed;
            public float BombTime;
            public int Power;
            public float Range;
            public Action PlayEndCallBack;
        }
        private Vector3 prevPoint = Vector3.zero;
        private Vector3 forward = Vector3.zero;
        private Vector3 hitPoint = Vector3.zero;
        private Vector3 hitNormal = Vector3.zero;
        private Quaternion hitRotation = Quaternion.identity;
        private RaycastHit[] raycastHits;
        private Action endCallBack;
        private bool isPlaying = false;
        private bool isHit = false;
        private int power = 0;
        private float range = 0;
        private int ignoreGpoID = 0;
        private float bombTime = 0.0f;
        private float moveSpeed = 0f;
        private ServerGPO hitGpo = null;
        private GPOData.PartEnum hitType = GPOData.PartEnum.Body;
        private S_Ability_Base abilitySystem;
        private Transform hitTransform = null;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.IgnoreGpoID, initData.MoveSpeed, initData.BombTime, initData.Power, initData.Range, initData.PlayEndCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            abilitySystem = (S_Ability_Base)mySystem;
            forward = iEntity.GetRota() * Vector3.forward;
            prevPoint = iEntity.GetPoint();
            raycastHits = new RaycastHit[10];
            isPlaying = true;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            abilitySystem = null;
            raycastHits = null;
            hitGpo = null; 
        }

        public void SetData(int ignoreID, float speed, float bombTime, int power, float range, Action PlayEndCallBack) {
            if (PlayEndCallBack == null) {
                Debug.LogWarning("PlayEndCallBack is null.");
            }
            this.range = range;
            this.moveSpeed = speed;
            this.ignoreGpoID = ignoreID;
            this.endCallBack = PlayEndCallBack;
            this.bombTime = bombTime;
            this.power = power;
        }

        private void OnUpdate(float delta) {
            if (!isPlaying) return;
            if (isHit) {
                HandleBombTimer(delta);
            } else {
                Move(delta);
                HitCheck();
            }
        }

        private void HandleBombTimer(float delta) {
            if (bombTime <= 0) return;
            bombTime -= delta;
            if (bombTime <= 0) {
                PlayExplosiveAE();
                endCallBack?.Invoke();
            }
        }

        private void Move(float delta) {
            var point = iEntity.GetPoint();
            point += forward * this.moveSpeed * delta;
            iEntity.SetPoint(point);
        }

        private void HitCheck() {
            var direction = iEntity.GetRota() * Vector3.forward;
            var distance = Vector3.Distance(iEntity.GetPoint(), prevPoint);
            var count = Physics.RaycastNonAlloc(prevPoint, direction, raycastHits, distance, ~(LayerData.ClientLayerMask));
            if (count > 0) {
                SendHitGPO(count, raycastHits, prevPoint, distance);
            }
            if (iEntity != null) {
                prevPoint = iEntity.GetPoint();
            }
        }

        public void SendHitGPO(int count, RaycastHit[] list, Vector3 startPoint, float maxDistance) {
            var closestDistance  = maxDistance;
            HitType closestHitType  = null;
            for (int i = 0; i < count; i++) {
                var ray = list[i];
                if (ray.collider == null) continue;
                var gameObject = ray.collider.gameObject;
                var hitType = gameObject.GetComponent<HitType>();
                if (hitType != null && ShouldIgnoreHit(hitType)) continue;
                
                var distance = ray.distance;
                if (closestDistance  > distance) {
                    closestHitType  = hitType;
                    closestDistance  = distance;
                    hitPoint = ray.point;
                    isHit = true;
                }
            }
            if (isHit == false) {
                return;
            }
            hitNormal = (startPoint - hitPoint).normalized;
            if (closestHitType != null && closestHitType.MyEntity != null && closestHitType.MyEntity.GetGPO() != null) {
                hitTransform = closestHitType.transform;
                hitGpo = (ServerGPO)closestHitType.MyEntity.GetGPO();
                hitType = closestHitType.Part;
                hitRotation = Quaternion.LookRotation(hitTransform.InverseTransformDirection(hitNormal));
                hitPoint = hitTransform.InverseTransformPoint(hitPoint); // 获得转换后的局部坐标
            } else {
                hitRotation = Quaternion.LookRotation(hitNormal);
            }
            RpcHitData();
        }

        private bool ShouldIgnoreHit(HitType hitType) {
            return hitType.Layer == GPOData.LayerEnum.Ignore || 
                   (hitType.MyEntity != null && hitType.MyEntity.GetGPOID() == ignoreGpoID);
        }

        private void RpcHitData() {
            var hitData = new Proto_Ability.Rpc_PenetratorGrenadeHit {
                abilityId = abilitySystem.AbilityId,
                hitGpoID = hitGpo != null ? hitGpo.GetGpoID() : 0,
                hitGpoType = hitGpo != null ? (int)hitType : 0,
                hitPoint = hitPoint,
                hitRota = hitRotation
            };
            Rpc(hitData);
        }

        private void PlayExplosiveAE() {
            if (hitTransform && hitGpo != null) {
                hitPoint = hitTransform.TransformPoint(hitPoint); // 局部坐标在转换为事件坐标
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = abilitySystem.FireGPO,
                MData = AbilityM_Explosive.CreateForID(AbilityM_Explosive.ID_BulletRPGExplosive),
                InData = new AbilityIn_Explosive {
                    In_StartPoint = hitPoint,
                    In_Hurt = power,
                    In_Range = range,
                }
            });
        }
    }
}