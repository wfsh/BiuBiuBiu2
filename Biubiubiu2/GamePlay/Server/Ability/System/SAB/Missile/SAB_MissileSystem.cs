using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_MissileSystem : S_Ability_Base {
        private AbilityData.PlayAbility_Missile inData;

        protected override void OnAwake() {
            inData = (AbilityData.PlayAbility_Missile)MData;
            AddComponents();
            FireGPO.Dispatcher(new SE_Skill.Event_SetSkillInProgressForAbility {
                AbilityId = AbilityId,
                isSkillInProgress = true
            });
        }

        protected override void OnClear() {
            FireGPO.Dispatcher(new SE_Skill.Event_SetSkillInProgressForAbility {
                AbilityId = AbilityId,
                isSkillInProgress = false
            });
        }

        protected override void AddComponents() {
            // 服务器同步组件
            AddComponent<ServerNetworkSync>(new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            // 导弹生命周期
            AddComponent<ServerAbilityMissileLifeCycle>();
            // 移动组件
            AddComponent<MoveGrenade>(new MoveGrenade.InitData {
                Speed = inData.M_Speed,
                Points = inData.In_Points,
                CallBack = () => {
                    Dispatcher(new SE_Ability.Ability_MoveGrenadeEnd());
                }
            });
            // 导弹生成组件
            AddComponent<ServerAbilityMissileBombSpawner>(new ServerAbilityMissileBombSpawner.InitData {
                FireGPO = FireGPO,
                InData = inData
            });
        }

        void OnSyncSpawnProto(ServerNetworkSync sync) {
            var isMoveEnd = false;
            Dispatcher(new SE_Ability.Ability_GetMissileMoveing() {
                CallBack = b => {
                    isMoveEnd = !b;
                } 
            });
            var fireGpoId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = fireGpoId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_Ability.TargetRpc_Missile {
                    skinItemId = inData.In_SkinItemId,
                    points = inData.In_Points,
                    isMoveEnd = isMoveEnd,
                    areaRadius = inData.In_FinalAreaRadius,
                    abilityModId = inData.GetConfigId()
                })
            });
        }
    }
}