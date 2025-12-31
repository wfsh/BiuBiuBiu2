using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_FireGunFireSystem : S_Ability_Base {
        private AbilityData.PlayAbility_FireGunFire _inData;

        public AbilityData.PlayAbility_FireGunFire abilityData {
            get { return _inData; }
        }

        private ushort mGPOId;

        protected override void OnAwake() {
            base.OnAwake();
            _inData = (AbilityData.PlayAbility_FireGunFire)MData;
            // Debug.LogError("SAB_FireGunFireSystem  OnAwake");
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void AddComponents() {
            AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            AddSelect();
            AddHit();
            AddComponent<ServerAbilityChangeFireGunFireRange>();
        }

        private void AddSelect() {
            AddComponent<FireGunBulletSelectGPO>(new SphereRaycastHit.InitData {
                IgnoreTeamId = FireGPO.GetTeamID(),
                IgnoreGpoID = FireGPO.GetGpoID(),
                Range = _inData.M_Range,
                Radius = _inData.M_Radius,
                HitCallBack = null,
            });
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                    HitSplatter = AbilityM_PlayBloodSplatter.ID_BloodSplatter,
                    DamageType = DamageType.Normal,
                    Power = _inData.M_Power,
                    WeaponItemId = _inData.In_WeaponItemId,
                    AttnMaps = _inData.In_BulletAttnMap,
                    MaxDistance = _inData.M_Range,
                });
        }

        void OnSyncSpawnProto(ServerNetworkSync sync) {
            mGPOId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = mGPOId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_Ability.Rpc_PlayFireGunFire {
                    abilityModId = _inData.ConfigId,
                    range = _inData.M_Range,
                    initRange = _inData.M_InitRange
                })
            });
        }
    }
}