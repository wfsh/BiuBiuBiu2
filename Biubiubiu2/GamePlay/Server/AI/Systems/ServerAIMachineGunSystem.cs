using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIMachineGunSystem : S_AI_Base {
        private GPOM_MachineGun useMData;
        protected override void OnAwake() {
            useMData = (GPOM_MachineGun)MData;
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<ServerAIAttribute>(new ServerAIAttribute.InitData {
                ATK = useMData.Atk,
                AttackRange = useMData.AttackRange,
                MaxHp = useMData.Hp,
            });
            AddComponent<ServerAIMachineGunAttack>();
            AddComponent<ServerAIMachineGunHeadRota>();
            AddComponent<ServerMachineGunSetToGround>();
            AddComponent<ServerAIFindInsightTarget>(new ServerAIFindInsightTarget.InitData {
                CheckDistance = useMData.MaxAttackDistance,
                LayerMask = LayerData.ServerLayerMask | LayerData.DefaultLayerMask,
                IgnoreTeamId = TeamId,
                IgnoreCollierTrigger = false,
            });
        }
        
        protected override void OnStart() {
            base.OnStart();
            CreateEntity(AttributeData.Sign + "Server");
        }
    }
}