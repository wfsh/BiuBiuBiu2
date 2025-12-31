using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityGoldJokerCardTrickSpawner : ComponentBase {
        private enum State {
            Warn,
            Play,
            Next,
            Over
        }

        private ServerGPO fireGPO;
        private SAB_GoldJokerCardTrickSpawnerSystem abSystem;
        private AbilityM_GoldJokerCardTrickSpawner useMData;
        private float timer;
        private int index;
        private State state;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_GoldJokerCardTrickSpawnerSystem)mySystem;
            fireGPO = abSystem.FireGPO;
            useMData = abSystem.useMData;
            state = State.Warn;
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(Update);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_PlayEffect.CreateForID((byte)useMData.M_StartEffectId),
                InData = new AbilityIn_PlayEffect() {
                    In_StartPoint = fireGPO.GetPoint(),
                    In_StartRota = fireGPO.GetRota(),
                }
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossjorkerFlyKnifeStart,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = 1.6f,
                }
            });
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(Update);
            fireGPO = null;
        }

        private void Update(float delta) {
            timer -= delta;
            if (timer <= 0) {
                switch (state) {
                    case State.Warn:
                        timer = useMData.M_WarningTime[index];
                        PlayWarning();
                        fireGPO.Dispatcher(new SE_AI.Event_GoldJokerLookAtTarget() {
                            IsLooking = false
                        });
                        state = State.Play;
                        break;
                    case State.Play:
                        PlayFinesse();
                        state = State.Next;
                        break;
                    case State.Next:
                        if (++index < useMData.M_EffectId.Length) {
                            timer =  useMData.M_PostTime[index - 1];
                            state = State.Warn;
                            fireGPO.Dispatcher(new SE_AI.Event_GoldJokerLookAtTarget() {
                                IsLooking = true
                            });
                        } else {
                            state = State.Over;
                        }
                        break;
                }
            }
        }

        private void PlayWarning() {
            fireGPO.Dispatcher(new SE_AI.Event_PlayBossAnim() {
                Id = useMData.M_AnimId[index]
            });
            var finesseCount = useMData.M_FinesseCount[index];
            if (finesseCount > 0) {
                var angle = 360 / finesseCount;
                for (int i = 0; i < finesseCount; i++) {
                    Quaternion rot = Quaternion.Euler(0, angle * i, 0);
                    rot *= fireGPO.GetRota();
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                        FireGPO = fireGPO,
                        AbilityMData = new AbilityData.PlayAbility_PlayWarningEffect() {
                            ConfigId = AbilityConfig.PlayRectWarningEffect,
                            In_StartPoint = fireGPO.GetPoint() + rot * useMData.M_WarningOffset[index],
                            In_StartLookAt = rot * Vector3.forward,
                            In_StartScale = new Vector3(2 * useMData.M_AttackRadius[index], 1, useMData.M_MaxDistance[index]),
                            In_LifeTime = useMData.M_WarningTime[index],
                        }
                    });
                }
            }
        }

        private void PlayFinesse() {
            var finesseCount = useMData.M_FinesseCount[index];
            if (finesseCount > 0) {
                var angle = 360 / finesseCount;
                for (int i = 0; i < finesseCount; i++) {
                    var rot = fireGPO.GetRota() * Quaternion.Euler(useMData.M_PitchAngle[index], angle * i, 0);
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = fireGPO,
                        MData = AbilityM_MoveRangeHurt.CreateForID((byte)useMData.M_EffectId[index]),
                        InData = new AbilityIn_MoveRangeHurt() {
                            In_StartPoint = fireGPO.GetPoint() + rot * useMData.M_AttackOffset[index],
                            In_MoveSpeed = useMData.M_MoveSpeed[index],
                            In_ATK = useMData.M_ATK[index],
                            In_LifeTime = useMData.M_MaxDistance[index] / useMData.M_MoveSpeed[index],
                            In_Rangle = useMData.M_AttackRadius[index],
                            In_StartDir = rot * Vector3.forward,
                            In_MaxDistance = useMData.M_MaxDistance[index]
                        }
                    });
                }
            }
        }
    }
}