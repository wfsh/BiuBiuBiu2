using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public partial class ClientAbilityManager {
        protected C_Ability_Base HandleAbilityABType(string protoID, Action<C_Ability_Base> callBack) {
            C_Ability_Base system = null;
            switch (protoID) {
                case Proto_AbilityAB_Auto.Rpc_Bullet.ID:
                    system = AddSystem<CAB_BulletSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_PlayHollowCircleWarningEffect.ID:
                    system = AddSystem<CAB_PlayHollowCircleWarningEffectSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_PlayMovingEffect.ID:
                    system = AddSystem<CAB_PlayMovingEffectSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_PlayWWiseAudio.ID:
                    system = AddSystem<CAB_PlayWWiseAudioSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_Grenade.ID:
                    system = AddSystem<CAB_GrenadeSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_SniperAlertEffect.ID:
                    system = AddSystem<CAB_SniperAlertEffectSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_SniperLaserEffect.ID:
                    system = AddSystem<CAB_SniperLaserEffectSystem>(callBack);
                    break;
            }
            return system;
        }
    }
}
