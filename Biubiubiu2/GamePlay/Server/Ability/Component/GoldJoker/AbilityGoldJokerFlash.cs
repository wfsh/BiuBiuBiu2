using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityGoldJokerFlash : ComponentBase {
        private SAB_GoldJokerFlashSystem abSystem;
        private IGPO fireGPO;
        private AbilityIn_GoldJokerFlash useInData;
        private AbilityM_GoldJokerFlash useMData;
        private float timer;
        private bool isPlayEffect;

        protected override void OnAwake() {
            abSystem = (SAB_GoldJokerFlashSystem)mySystem;
            useInData = (AbilityIn_GoldJokerFlash)abSystem.InData;
            useMData = (AbilityM_GoldJokerFlash)abSystem.MData;
            fireGPO = abSystem.FireGPO;
        }

        protected override void OnStart() {
            timer = useMData.M_StartTime;
            AddUpdate(OnUpdate);
            fireGPO.Dispatcher(new SE_AI.Event_PlayBossAnim() {
                Id = useMData.M_AnimId
            });
        }

        protected override void OnClear() {
            base.OnClear();
            fireGPO = null;
        }

        private void OnUpdate(float delta) {
            timer -= delta;
            if (timer <= 0) {
                if (!isPlayEffect) {
                    Teleport();
                    isPlayEffect = true;
                }
            }
        }

        private void Teleport() {
            var startPoint = fireGPO.GetPoint();
            fireGPO.Dispatcher(new SE_AI_Joker.Event_SetGoldJokerFlashTeleport {
                Point = useInData.In_EndPoint,
            });
            
            var endPoint = useInData.In_EndPoint;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayMovingEffect.CreateForID((byte)useMData.M_EffectId),
                InData = new AbilityIn_PlayMovingEffect() {
                    In_StartPoint = startPoint,
                    In_StartLookAt = endPoint - startPoint,
                    In_StartScale = Vector3.one,
                    In_LifeTime = useMData.M_LifeTime - useMData.M_StartTime,
                    In_MoveDir = (endPoint - startPoint).normalized,
                    In_MoveSpeed = (endPoint - startPoint).magnitude / (useMData.M_LifeTime - useMData.M_StartTime),
                    In_AudioKey = 0,
                }
            });
        }
    }
}
