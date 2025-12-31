using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityBoxRectAttack : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float AttackRange;
            public float ATK;
        }
        private AbilityM_BoxRectAttack mData;
        private IGPO fireGpo;
        private RaycastHit[] attackBoxHits = new RaycastHit[10];
        private HashSet<int> hitGpoSet = new HashSet<int>();
        private float attackRange = 0f;
        private float atk = 0f;
        private float delayTime = 0f;

        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            attackRange = initData.AttackRange;
            atk = initData.ATK;
            var abSystem = (S_Ability_Base)mySystem;
            mData = (AbilityM_BoxRectAttack)abSystem.MData;
            fireGpo = abSystem.FireGPO;
            AddUpdate(OnUpdate);
        }

        protected override void OnStart() {
            base.OnStart();
            PlayWWiseAudio(mData.M_PlayWwiseId);
            delayTime = mData.M_AttackDelay;
        }

        private void PlayWWiseAudio(int wwiseId) {
            if (wwiseId <= 0) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGpo,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = (ushort)mData.M_PlayWwiseId,
                    In_StartPoint = iEntity.GetPoint(),
                    In_LifeTime = 1f,
                }
            });
        }
        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            base.OnClear();
        }

        private void OnUpdate(float deltaTime) {
            if (delayTime > 0f) {
                delayTime -= deltaTime;
                if (delayTime <= 0f) {
                    delayTime = mData.M_AttackDelay;
                    Attack();
                }
            }
        }

        private void Attack() {
            if (fireGpo == null  || fireGpo.IsDead() || atk <= 0f || attackRange <= 0f) {
                Debug.LogError("SAB attack:" + atk + " Range:" + attackRange);
                return;
            }
            // 伤害检测
            Array.Clear(attackBoxHits, 0, attackBoxHits.Length);
            var count = Physics.BoxCastNonAlloc(iEntity.GetPoint(), mData.M_HalfExtend,
                iEntity.GetForward(), attackBoxHits, Quaternion.identity, attackRange,
                LayerData.ServerLayerMask);
#if UNITY_EDITOR
            DebugDrawAttack();
#endif
            for (int i = 0; i < count; i++) {
                var raycastHit = attackBoxHits[i];
                var collider = raycastHit.collider;
                if (collider == null) {
                    continue;
                }
                var hitType = collider.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore || hitType.MyEntity == null) {
                        continue;
                    }
                    var gpo = hitType.MyEntity.GetGPO();
                    if (gpo == null || gpo.IsDead()) {
                        continue;
                    }
                    var gpoId = gpo.GetGpoID();
                    if (gpoId == fireGpo.GetGpoID() || hitGpoSet.Contains(gpoId)) {
                        continue;
                    }
                    hitGpoSet.Add(gpoId);
                    var hitPoint = raycastHit.collider.ClosestPoint(iEntity.GetPoint());
                    gpo.Dispatcher(new SE_GPO.Event_GPOHurt {
                        Hurt = Mathf.CeilToInt(atk),
                        IsHead = false,
                        AttackGPO = fireGpo,
                        AttackItemId = mData.M_ItemId
                    });
                    PlayHitEffect(hitPoint);
                    PlayWWiseAudio(mData.M_HitWwiseId);
                }
            }
        }
        
        private void PlayHitEffect(Vector3 hitPoint) {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                FireGPO = fireGpo,
                MData = AbilityM_PlayEffect.CreateForID((byte)mData.M_HitAbilityEffect),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = hitPoint, In_StartRota = Quaternion.identity
                },
            });
        }

        // 绘制一个盒子表示攻击范围
        private void DebugDrawAttack() {
            var attackOrigin = iEntity.GetPoint();
            var attackDirection = iEntity.GetForward();
            var halfSize = mData.M_HalfExtend;
            var boxCenter = attackOrigin + attackDirection * attackRange;
            var rotation = Quaternion.LookRotation(attackDirection);
            DebugDrawBox(attackOrigin, halfSize * 2, rotation, Color.red, 10);
            DebugDrawBox(boxCenter, halfSize * 2, rotation, Color.red, 10);
            Debug.DrawRay(iEntity.GetPoint(), iEntity.GetForward() * attackRange, Color.red, 10);
        }

        private void DebugDrawBox(Vector3 center, Vector3 size, Quaternion rotation, Color color, float duration) {
            var extents = size * 0.5f;
            var right = rotation * Vector3.right * extents.x;
            var up = rotation * Vector3.up * extents.y;
            var forward = rotation * Vector3.forward * extents.z;

            // 8 个顶点
            var vertices = new Vector3[8];
            vertices[0] = center + right + up + forward;
            vertices[1] = center - right + up + forward;
            vertices[2] = center - right - up + forward;
            vertices[3] = center + right - up + forward;
            vertices[4] = center + right + up - forward;
            vertices[5] = center - right + up - forward;
            vertices[6] = center - right - up - forward;
            vertices[7] = center + right - up - forward;

            // 12 条边
            Debug.DrawLine(vertices[0], vertices[1], color, duration);
            Debug.DrawLine(vertices[1], vertices[2], color, duration);
            Debug.DrawLine(vertices[2], vertices[3], color, duration);
            Debug.DrawLine(vertices[3], vertices[0], color, duration);
            Debug.DrawLine(vertices[4], vertices[5], color, duration);
            Debug.DrawLine(vertices[5], vertices[6], color, duration);
            Debug.DrawLine(vertices[6], vertices[7], color, duration);
            Debug.DrawLine(vertices[7], vertices[4], color, duration);
            Debug.DrawLine(vertices[0], vertices[4], color, duration);
            Debug.DrawLine(vertices[1], vertices[5], color, duration);
            Debug.DrawLine(vertices[2], vertices[6], color, duration);
            Debug.DrawLine(vertices[3], vertices[7], color, duration);
        }
    }
}