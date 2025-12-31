using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIJokerUavSystem : S_AI_Base {
        private GPOM_JokerUav useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_JokerUav)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity("JokerUAVServer");
            
        }
        protected override void AddComponents() {     
            base.AddComponents();
            AddComponent<ServerAIAttribute>(new ServerAIAttribute.InitData { // 怪物属性
                ATK = 100,
                AttackRange = 0,
                MaxHp = useMData.Hp,
            });
            AddComponent<ServerGoldDashAIHateTarget>();
            if (WarReportData.IsStartSausageWarReport == false) {
                var behaviorData = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(useMData.Sign);
                AddComponent<ServerAIBehaviour>(new ServerAIBehaviour.InitData {
                    AIBehaviour = behaviorData.BehaviorSign,
                    PetAIBehaviour = behaviorData.BehaviorSign,
                });
            }
            AddComponent<ServerGoldDashAIActivate>(); // 激活状态管理（警戒/战斗）
            AddComponent<ServerGoldDashAISight>(); // 怪物视野组件
            AddComponent<ServerAIGoldJokerShield>();
            AddComponent<ServerJokerUAVMove>(); // AI 移动
            AddComponent<ServerAIJokerUAVAttack>();// 无人机攻击
            AddComponent<ServerGoldDashJokerUAVState>();
            AddComponent<ServerAIGoldJokerUAVAnim>();
        }
    }
}