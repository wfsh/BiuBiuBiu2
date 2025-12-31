using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SplitConeAttack : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityData.PlayAbility_SplitCone AbilityData;
            public IGPO FireGPO;
        }
        private int randomAtk;
        private IGPO fireGPO;
        private AbilityData.PlayAbility_SplitCone inData;
        
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            Init(initData.AbilityData, initData.FireGPO);
        }

        protected override void OnStart() {
            SpawnBullets();
        }

        public void Init(AbilityData.PlayAbility_SplitCone abilityData, IGPO fireGPO) {
            inData = abilityData;
            this.fireGPO = fireGPO;
            fireGPO.Dispatcher(new SE_GPO.Event_GetRandomATK {
                CallBack = randomAtk => this.randomAtk = randomAtk
            });
        }

        private void SpawnBullets() {
            var coneRadian = (inData.M_ConeAngle - inData.In_DiffusionReductionAngle) / 2 * Mathf.Deg2Rad;
            var coneHeight = Mathf.Cos(coneRadian) * inData.In_MoveDistance;
            var coneRadius = Mathf.Tan(coneRadian) * coneHeight;
            var forwardRot = Quaternion.Euler(inData.In_ForwardAngle);
            var coneCenterPos = inData.In_StartPoint + forwardRot * Vector3.forward * coneHeight;

            foreach (var point in inData.M_BulletSpreadPoints) {
                var endPoint = coneCenterPos + forwardRot * (coneRadius * point);
                MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                    FireGPO = fireGPO,
                    AbilityMData = new AbilityData.PlayAbility_BulletWithStartPos {
                        ConfigId = AbilityConfig.BulletSplitSubBullet,
                        In_StartPoint = inData.In_StartPoint,
                        In_TargetPoint = endPoint,
                        In_Speed = inData.In_Speed,
                        In_MoveDistance = inData.In_MoveDistance,
                        In_WeaponItemId = inData.In_WeaponItemId,
                        In_RandomAtk = randomAtk
                    },
                });
            }
        }

        private Vector3 GetOffset(float radian, float radius) {
            return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0) * radius;
        }
    }
}