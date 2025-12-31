using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIGiantDaDaSystem : S_AI_Base {
        private GPOM_GiantDaDa useMData;
        protected override void OnAwake() {
            useMData = (GPOM_GiantDaDa)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(useMData.Sign + "Server");
        }

        protected override void AddComponents() {   
            base.AddComponents();       
            AddComponent<ServerAIAttribute>(new ServerAIAttribute.InitData {
                ATK = 100,
                AttackRange = 0,
                MaxHp = useMData.Hp,
            }); 
            AddComponent<ServerAIGiantDaDaAbilityTimeline>(); // 巨人达达技能时间轴组件
            AddComponent<ServerGiantDaDaAnim>(); // 巨人达达动画组件
            AddComponent<ServerAIGiantDaDaFightRange>(); // 巨人达达战斗范围组件
            AddComponent<ServerAIGiantDaDaLifeCycle>(); // 巨人达达生命周期组件
            AddComponent<ServerGoldDashAIHateTarget>();
            if (WarReportData.IsStartSausageWarReport == false) {
                var behaviorData = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(useMData.Sign);
                AddComponent<ServerAIBehaviour>(new ServerAIBehaviour.InitData {
                    AIBehaviour = behaviorData.BehaviorSign,
                    PetAIBehaviour = behaviorData.BehaviorSign,
                }); 
            }
        }
    }
}