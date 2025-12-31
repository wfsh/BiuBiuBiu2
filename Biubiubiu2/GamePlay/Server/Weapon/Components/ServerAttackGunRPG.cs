using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAttackGunRPG : ComponentBase {
        private IGPO fireGPO;
        protected override void OnAwake() {
            mySystem.Register<SE_Weapon.Event_PlayFireAbility>(OnPlayFireAbilityBack);
        }

        protected override void OnStart() {
            var system = (S_Weapon_Base)mySystem;
            fireGPO = system.UseGPO();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Weapon.Event_PlayFireAbility>(OnPlayFireAbilityBack);
        }

        private void OnPlayFireAbilityBack(ISystemMsg body, SE_Weapon.Event_PlayFireAbility ent) {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                FireGPO = fireGPO,
                AbilityMData = new AbilityData.PlayAbility_RPG {
                    ConfigId = AbilityConfig.BulletRPG, In_StartPoint = ent.StartPoint, In_TargetPoint = ent.TargetPoint
                }
            });
        }
    }
}