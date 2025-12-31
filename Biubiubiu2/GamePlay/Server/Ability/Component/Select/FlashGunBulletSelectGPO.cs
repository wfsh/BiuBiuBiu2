using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class FlashGunBulletSelectGPO : MovePointRaycastHit {
        private IGPO fireGPO;
        private AbilityIn_Bullet useInData;
        public class InitData : MovePointRaycastHit.InitData {
            public AbilityIn_Bullet AbilityMData;
        }

        protected override void OnAwake() {
            base.OnAwake();
            if (initDataBase != null) {
                var initData = (InitData)initDataBase;
                SetModData(initData.AbilityMData);
            }
        }

        protected override void OnStart() {
            base.OnStart();
            SetLayerMask(LayerData.ServerLayerMask | LayerData.DefaultLayerMask);
            fireGPO = ((S_Ability_Base)mySystem).FireGPO;
            SetIgnoreTeamId(fireGPO.GetTeamID());
        }

        public void SetModData(AbilityIn_Bullet data) {
            this.useInData = data;
        }
        protected override void OnHitGameObj(GameObject gameObj, RaycastHit hitRay) {
            var hitType = gameObj.GetComponent<HitType>();
            
            if (hitType != null && hitType.MyEntity != null) {
                var hitGPO = hitType.MyEntity.GetGPO();
                if (hitGPO != null) {
                    var checkHitRatio = Random.Range(0f, 1f);
                    var hitRatio = 1f;
                    fireGPO.Dispatcher(new SE_GPO.Event_GetHitRatio {
                        HitGpoType = hitGPO.GetGPOType(),
                        CallBack = (ratio) => {
                            hitRatio = ratio;
                        }
                    });
                    if (checkHitRatio > hitRatio) {
                        return;
                    }

                    FlashHitGPO(hitGPO,hitRay);
                }
            }
            return;
        }

        private void FlashHitGPO(IGPO hitGPO,RaycastHit hitRay) {
            mySystem.Dispatcher(new SE_Ability.HitGPO {
                hitGPO = hitGPO,
                hitPoint = hitRay.point,
                HurtRatio = 1f,
                CallBack = damage => {
                    if (damage > 1) {
                        MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                            CallBack = null,
                            FireGPO = fireGPO,
                            AbilityMData = new AbilityData.PlayAbility_RangeFlash() {
                                ConfigId = AbilityConfig.FlashGunExplosive,
                                In_StartPoint = hitRay.point,
                                M_Power = damage / 2, //增加配置可以配置比例
                                In_WeaponId = useInData.In_WeaponItemId,
                                In_IngoreGPOId = hitGPO.GetGpoID()
                            }
                        });
                    }
                }
            });
        }
    }
}