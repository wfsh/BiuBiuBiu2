using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityHurtGPO : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public byte HitSplatter = AbilityM_PlayBloodSplatter.ID_BloodSplatter;
            public DamageType DamageType = DamageType.Normal; // 0 普通伤害 1 灼烧伤害
            public int Power = 0;
            public int WeaponItemId = 0;
            public List<WeaponData.BulletAttnMap> AttnMaps = new List<WeaponData.BulletAttnMap>();
            public float MaxDistance = 0;
            public int Atk = 0;
        }
        private int fireATK = 0;
        private float hitHeadRate = 1f;
        private float addEffectFireDistanceRate = 0f;
        private int power = 0;
        private S_Ability_Base abSystem;
        private List<WeaponData.BulletAttnMap> attnMaps;
        private float maxDistance;
        private Vector3 startPoint = Vector3.zero;
        private int weaponItemId;
        private byte clientHurtEffectAbilityConfigId = AbilityM_PlayBloodSplatter.ID_BloodSplatter;
        private DamageType damageType;
        private IGPO fireGpo;
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (S_Ability_Base)mySystem;
            fireGpo = abSystem.FireGPO;
            mySystem.Register<SE_Ability.HitGPO>(HitCallBack);
            var initData = (InitData)initDataBase;
            SetHurtType(initData.HitSplatter,initData.DamageType);
            SetPower(initData.Power, initData.WeaponItemId);
            SetBulletAttnMap(initData.AttnMaps, initData.MaxDistance);
            SetAtk(initData.Atk);
        }

        protected override void OnStart() {
            startPoint = iEntity.GetPoint();
            if (fireATK <= 0) {
                fireGpo.Dispatcher(new SE_GPO.Event_GetATK {
	                CallBack = OnFireAtkCallBack
	            });
            }
            fireGpo.Dispatcher(new SE_GPO.Event_GetHeadHurtRate() {
                CallBack = OnGetHitHeadHurtRateCallBack
            });
            fireGpo.Dispatcher(new SE_GPO.Event_GetAddEffectFireDistanceRate {
                CallBack = OnGetEffectFireDistance
            });
        }
        
        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Ability.HitGPO>(HitCallBack);
        }

        public void SetPower(int power, int weaponItemId) {
            this.power = power;
            this.weaponItemId = weaponItemId;
        }

        public void SetBulletAttnMap(List<WeaponData.BulletAttnMap> attnMaps, float maxDistance) {
            this.attnMaps = attnMaps;
            this.maxDistance = maxDistance;
        }

        public void SetAtk(int atk) {
            fireATK = atk;
        }

        private void OnFireAtkCallBack(int atk) {
            fireATK = atk;
        }
        
        private void OnGetHitHeadHurtRateCallBack(float multiplier) {
            hitHeadRate = multiplier;
        }

        private void OnGetEffectFireDistance(float ratio) {
            addEffectFireDistanceRate = ratio;
        }

        void HitCallBack(ISystemMsg body, SE_Ability.HitGPO entData) {
            if (entData.hitGPO.IsClear()) {
                return;
            }

            var hitPoint = entData.hitPoint;
            if (hitPoint == Vector3.zero) {
                var tran = entData.hitGPO.GetBodyTran(GPOData.PartEnum.Head);
                if (tran != null) {
                    hitPoint = tran.position;
                } else {
                    hitPoint = entData.hitGPO.GetPoint();
                    hitPoint.y += 0.5f;
                }
            }
            var hurt = Hit(entData.hitGPO, entData.HurtRatio, entData.isHead, entData.SourceAbilityType);
            if (hurt > 0) {
                if (entData.CallBack != null) {
                    entData.CallBack.Invoke(hurt);
                }
                PlayEffect(entData.hitGPO, hitPoint, hurt);
            }
        }

        private int Hit(IGPO hitGPO, float hurtRatio, bool isHitHead, string sourceAbilityType) {
            IGPO fireGPO = abSystem.FireGPO;

            if (fireGPO == null || fireGPO.IsClear() || hitGPO.IsDead() || hitGPO.IsGodMode()) {
                return -1;
            }
            fireGPO.Dispatcher(new SE_GPO.Event_GetATK {
                CallBack = OnFireAtkCallBack
            });
            
            fireGPO.Dispatcher(new SE_GPO.Event_GetHeadHurtRate() {
                CallBack = OnGetHitHeadHurtRateCallBack
            });
            if (fireATK == 0) {
#if UNITY_EDITOR
                Debug.LogError($"攻击力不会是 0  - {abSystem.FireGPO.GetName()}");
#endif
                fireATK = 1;
            }

            if (hurtRatio <= 0f) {
                hurtRatio = 1f;
            }
            var hurtPoint = hitGPO.GetPoint();
            var hurt = Mathf.CeilToInt(fireATK * power * hurtRatio * 0.01f);
            hurt = Mathf.Max(1, Mathf.CeilToInt(hurt * CalculateHurt(hurtPoint)));
            var distance = Vector3.Distance(abSystem.FireGPO.GetPoint(), hurtPoint);
            abSystem.FireGPO.Dispatcher(new SE_Behaviour.Event_HateFindTarget {
                TargetGPO = hitGPO, Distance = distance,
            });
            hitGPO.Dispatcher(new SE_Behaviour.Event_HateLockTarget {
                TargetGPO = abSystem.FireGPO, Distance = distance,
            });
            var hitHandAddHurt = isHitHead ? hitHeadRate : 1;
            var hitValue = Mathf.CeilToInt(hitHandAddHurt * hurt);
            hitGPO.Dispatcher(new SE_GPO.Event_GPOHurt {
                Hurt = hitValue,
                IsHead = isHitHead,
                AttackGPO = fireGPO,
                AttackItemId = weaponItemId
            });

            if (fireGPO.GetGPOType() == GPOData.GPOType.AI) {
                var quality = (AIData.AIQuality)fireGPO.GetMData().GetQuality();
                if (quality == AIData.AIQuality.Boss) {
                    fireGPO.Dispatcher(new SE_AI.Event_BossAbilityHit() {
                        sourceAbilityType = sourceAbilityType,
                    });
                }
            }
            return hurt;
        }

        // 根据 attnMap， maxDistance 计算当前移动距离相比总距离的伤害衰减范围 [0, 1]
        private float CalculateHurt(Vector3 hitPoint) {
            if (attnMaps == null || attnMaps.Count == 0) {
                return 1f;
            }
            var nowDistance = Vector3.Distance(startPoint, hitPoint);
            var distanceRatio = Mathf.Min(1f, nowDistance / maxDistance);
            var len = attnMaps.Count;
            var attnData = attnMaps[len - 1];
            if (distanceRatio >= attnData.MaxDistanceRatio) {
                return attnData.MaxHurtRatio;
            } else {
                for (int i = 0; i < len; i++) {
                    var map = attnMaps[i];
                    var minDistanceRatio = map.MinDistanceRatio;
                    var maxDistanceRatio = map.MaxDistanceRatio;
                    if (minDistanceRatio > 0) {
                        minDistanceRatio += addEffectFireDistanceRate;
                    }

                    maxDistanceRatio += addEffectFireDistanceRate;
                    if (maxDistanceRatio > 1f) {
                        maxDistanceRatio = 1f;
                    }

                    if (minDistanceRatio <= distanceRatio && distanceRatio < maxDistanceRatio) {
                        // 计算当前距离的伤害衰减比例
                        var hurtRatio = map.MinHurtRatio + (map.MaxHurtRatio - map.MinHurtRatio) * (distanceRatio - minDistanceRatio) / (maxDistanceRatio - minDistanceRatio);
                        return hurtRatio;
                    }
                }
            }

            return 1f;
        }

        public void SetHurtType(byte hitEffectAbilityConfigId,DamageType damageType) {//0 普通伤害 1 灼烧伤害
            if (hitEffectAbilityConfigId <= 0) {
                hitEffectAbilityConfigId = AbilityM_PlayBloodSplatter.ID_BloodSplatter;
            }
            clientHurtEffectAbilityConfigId = hitEffectAbilityConfigId;
            this.damageType = damageType;
        }

        private void PlayEffect(IGPO hitGpo, Vector3 hitPoint, int hurt) {
            if (hitGpo.IsClear()) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                FireGPO = abSystem.FireGPO,
                MData = AbilityM_PlayBloodSplatter.CreateForID(clientHurtEffectAbilityConfigId),
                InData = new AbilityIn_PlayBloodSplatter {
                    In_HitGpoId = hitGpo.GetGpoID(),
                    In_HitPoint = hitPoint,
                    In_DiffPos = hitPoint - hitGpo.GetPoint(),
                    In_BloodValue = hurt,
                    In_HitItemId = weaponItemId
                }
            });
        }
    }
}