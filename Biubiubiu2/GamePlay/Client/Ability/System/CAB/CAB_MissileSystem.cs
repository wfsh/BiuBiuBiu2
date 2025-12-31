using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_MissileSystem : C_Ability_Base {
        private AbilityData.PlayAbility_Missile modData;
        private Proto_Ability.TargetRpc_Missile rpcData;

        protected override void OnAwake() {
            rpcData = (Proto_Ability.TargetRpc_Missile)InData;
            modData = (AbilityData.PlayAbility_Missile)AbilityConfig.GetAbilityModData(rpcData.abilityModId);
            AddComponent<ClientAbilityNetworkBehaviour>();
            AddComponent<ClientAbilityMissileVisual>(new ClientAbilityMissileVisual.InitData {
                FireGpoId = FireGpoId,
                RpcData = rpcData,
            });
        }

        protected override void OnStart() {
            CreateEntity(WeaponSkinData.GetSkinSfx(rpcData.skinItemId, modData.M_EffectSign));
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            if (iEnter == null) {
                Debug.LogError("[Error] CAB_MissileSystem 加载 Entity 失败:" + modData.M_EffectSign);
                return;
            }
            if (!rpcData.isMoveEnd) {
                AddComponent<ClientAbilityMissileMove>(new ClientAbilityMissileMove.InitData {
                    Points = rpcData.points,
                    Speed = modData.M_Speed,
                });
            } else {
                iEnter.SetPoint(rpcData.points[^1]);
                Dispatcher(new CE_Ability.MissileMoveEnd());
            }
        }
    }
}