using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    /// <summary>
    /// 常驻类型 AB 编写范例。支持重连，单独类型同步
    /// </summary>
    public class SAB_TestFire : S_Ability_Base {
        private AbilityData.PlayAbility_TestFire inData;

        protected override void OnAwake() {
            base.OnAwake();
            inData = (AbilityData.PlayAbility_TestFire)MData;
            iEntity.SetPoint(inData.In_StartPoint);
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ServerNetworkSync>(new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto, // 常驻类型
            });
            AddComponent<ServerAbilityTestFire>(); // 添加测试火焰组件
        }

        void OnSyncSpawnProto(ServerNetworkSync sync) {
            var fireGpoId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = fireGpoId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_Ability.TargetRpc_PlayTestFire {
                    startPoint = inData.In_StartPoint,
                })
            });
        }
    }
}