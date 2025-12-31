using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIGoldJokerSystem : S_AI_Base {
        private GPOM_AceJoker useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_AceJoker)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity("GoldJokerServer");
        }
        protected override void AddComponents() {    
            base.AddComponents();
            AddComponent<ServerAIAttribute>(new ServerAIAttribute.InitData {
                ATK = 100,
                AttackRange = 0,
                MaxHp = useMData.Hp,
            });
            AddComponent<ServerBossFightMusic>(new ServerBossFightMusic.InitData() {
                configId = AbilityM_BossFightMusic.ID_GoldJoker
            });
            AddComponent<ServerAIGoldJokerSkill>(new ServerAIGoldJokerSkill.InitData {
                bossType = 2,
            });
            AddComponent<ServerAIGoldJokerMove>();
            AddComponent<ServerAIGoldJokerTimeline>();
            AddComponent<ServerAIGoldJokerAnim>();
            AddComponent<ServerAIGoldJokerFightRange>();
        }
    }
}