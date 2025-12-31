using System;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityHurtGPODownHp : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public int HurtHp;
            public int HurtItemId;
            public DamageType DamageType = DamageType.Normal;
            public byte HitEffectAbilityConfigId = AbilityM_PlayBloodSplatter.ID_BloodSplatter;
        }
        private int hurtHp = 0;
        private S_Ability_Base abSystem;
        private int hurtItemId = 0;
        private byte clientHurtEffectAbilityConfigId = AbilityM_PlayBloodSplatter.ID_BloodSplatter;
        private DamageType damageType;
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (S_Ability_Base)mySystem;
            mySystem.Register<SE_Ability.HitGPO>(HitCallBack);
            var initData = (InitData)initDataBase;
            SetHurtHp(initData.HurtHp, initData.HurtItemId);
            SetHurtType(initData.HitEffectAbilityConfigId,initData.DamageType);
        }

        protected override void OnStart() {
        }

        public void SetHurtHp(int hurtHp, int itemId) {
            this.hurtHp = hurtHp;
            this.hurtItemId = itemId;
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Ability.HitGPO>(HitCallBack);
        }

        void HitCallBack(ISystemMsg body, SE_Ability.HitGPO entData) {
            if (entData.hitGPO.IsClear()) {
                return;
            }
             var hurt = Hit((ServerGPO)entData.hitGPO, entData.HurtRatio);
             if (hurt > 0) {
                 PlayEffect(entData.hitGPO, entData.hitPoint, hurt);
             }
        }

        private int Hit(ServerGPO hitGPO, float hurtRatio) {
            if (abSystem.FireGPO == null || abSystem.FireGPO.IsClear() || hitGPO.IsClear()) {
                return -1;
            }
            if (hurtHp == 0) {
                Debug.LogError("伤害不能为 0");
            }
            if (hurtRatio <= 0f) {
                hurtRatio = 1f;
            }
            var hurt = Mathf.CeilToInt(this.hurtHp * hurtRatio);
            var distance = Vector3.Distance(abSystem.FireGPO.GetPoint(), hitGPO.GetPoint());
            abSystem.FireGPO.Dispatcher(new SE_Behaviour.Event_HateFindTarget {
                TargetGPO = hitGPO, Distance = distance,
            });
            hitGPO.Dispatcher(new SE_Behaviour.Event_HateLockTarget {
                TargetGPO = abSystem.FireGPO, Distance = distance,
            });
            hitGPO.Dispatcher(new SE_GPO.Event_GPOHurt {
                Hurt = hurt,
                AttackGPO = abSystem.FireGPO,
                AttackItemId = hurtItemId,
                DamageType = damageType
            });
            return hurt;
        }
        public void SetHurtType(byte hitEffectAbilityConfigId,DamageType damageType) {
            if (hitEffectAbilityConfigId <= 0) {
                hitEffectAbilityConfigId = AbilityM_PlayBloodSplatter.ID_BloodSplatter;
            }
            clientHurtEffectAbilityConfigId = hitEffectAbilityConfigId;
            this.damageType = damageType;
        }
        private void PlayEffect(IGPO hitGpo,  Vector3 hitPoint, int hurt) {
            if (hitGpo.IsClear()) {
                return;
            }
            if (hitPoint == Vector3.zero) {
                var tran = hitGpo.GetBodyTran(GPOData.PartEnum.Head);
                if (tran != null) {
                    hitPoint = tran.position;
                } else {
                    hitPoint = hitGpo.GetPoint();
                    hitPoint.y += 0.5f;
                }
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = abSystem.FireGPO,
                MData = AbilityM_PlayBloodSplatter.CreateForID(clientHurtEffectAbilityConfigId),
                InData = new AbilityIn_PlayBloodSplatter {
                    In_HitGpoId = hitGpo.GetGpoID(),
                    In_HitPoint = hitPoint,
                    In_DiffPos = hitPoint - hitGpo.GetPoint(),
                    In_BloodValue = hurt,
                    In_HitItemId = hurtItemId
                }
            });
        }
    }
}