using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class FireGunBulletSelectGPO : SphereRaycastHit {
        private IGPO fireGPO;
        private SAB_FireGunFireSystem mSystem;

        protected override void OnStart() {
            base.OnStart();
            SetLayerMask(LayerData.ServerLayerMask | LayerData.DefaultLayerMask);
            fireGPO = ((S_Ability_Base)mySystem).FireGPO;
            SetIgnoreTeamId(fireGPO.GetTeamID());
            mSystem = ((SAB_FireGunFireSystem)mySystem);
        }

        protected override void OnClear() {
            base.OnClear();
            mSystem = null;
            fireGPO = null;
        }

        protected override bool OnHitGameObj(GameObject gameObj, RaycastHit hitRay) {
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null && hitType.MyEntity != null && hitType.MyEntity.GetGPO() != null) {
                var checkHitRatio = Random.Range(0f, 1f);
                var hitRatio = 1f;
                fireGPO.Dispatcher(new SE_GPO.Event_GetHitRatio {
                    HitGpoType = hitType.MyEntity.GetGPO().GetGPOType(),
                    CallBack = (ratio) => { hitRatio = ratio; }
                });
                if (checkHitRatio > hitRatio) {
                    return true;
                }

                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = hitType.MyEntity.GetGPO(),
                    hitPoint = hitRay.point,
                    HurtRatio = 1f,
                });
                if (hitType.MyEntity.GetGPO().IsDead() == false) {
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbilityEffect {
                        FireGPO = fireGPO,
                        TargetGPO = hitType.MyEntity.GetGPO(),
                        MData = AbilityM_HurtGPOByTime.CreateForID(mSystem.abilityData.M_HitAbilityEffect),
                        InData = new AbilityIn_HurtGPOByTime() {
                            In_WeaponItemId = mSystem.abilityData.In_WeaponItemId
                        },
                    });
                }
                return true;
            }

            InvokeRangeChangeCallback(hitRay.distance);
            return false;
        }
    }
}