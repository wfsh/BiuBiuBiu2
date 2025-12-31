using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityGoldJokerFloatingGunSpawner : ComponentBase {
        private SAB_GoldJokerFloatingGunSpawnerSystem abSystem;
        private ServerGPO fireGPO;
        private AbilityM_GoldJokerFloatingGunSpawner useMData;
        private float timer;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_GoldJokerFloatingGunSpawnerSystem)mySystem;
            useMData = abSystem.useMData;
            fireGPO = abSystem.FireGPO;
            fireGPO.Register<SE_GPO.Event_GetATK>(OnGetATKCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO.Unregister<SE_GPO.Event_GetATK>(OnGetATKCallBack);
            fireGPO = null;
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            timer = useMData.M_StartTime;
            fireGPO.Dispatcher(new SE_AI.Event_PlayBossAnim() {
                Id = AnimConfig_GoldDash_BOSSAceJoker.Anim_BOSSAceJoker_FloatingGun
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffect.CreateForID((byte)useMData.M_StartEffectId),
                InData = new AbilityIn_PlayEffect() {
                    In_StartPoint = fireGPO.GetPoint(),
                    In_StartRota = fireGPO.GetRota(),
                    In_LifeTime = useMData.M_StartTime
                }
            });
        }

        private void OnUpdate(float delta) {
            timer -= delta;
            if (timer <= 0) {
                if (useMData.M_GunCount > 0) {
                    var angle = 360 / useMData.M_GunCount;
                    for (int i = 0; i < useMData.M_GunCount; i++) {
                        Quaternion rot = Quaternion.Euler(0, angle * i, 0);
                        rot *= fireGPO.GetRota() * Quaternion.Euler(0, 90, 0);
                        MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                            FireGPO = fireGPO,
                            MData = AbilityM_GoldJokerFloatingGun.CreateForID((byte)useMData.M_GunEffectId),
                            InData = new AbilityIn_GoldJokerFloatingGun {
                                In_StartPoint = fireGPO.GetPoint() + rot * useMData.M_AttackOffset,
                                In_StartRot = rot,
                                In_Param = useMData
                            }
                        });
                    }
                }
                RemoveUpdate(OnUpdate);
            }
        }
        
        private void OnGetATKCallBack(ISystemMsg body, SE_GPO.Event_GetATK ent) {
            ent.CallBack.Invoke(useMData.M_ATK);
        }
    }
}