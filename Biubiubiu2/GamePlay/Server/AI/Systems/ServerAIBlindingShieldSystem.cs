using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIBlindingShieldSystem : S_AI_Base {
        private GPOM_BlindingShield useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_BlindingShield)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity("BlindingShieldServer");
        }

        protected override void AddComponents() {
            AddComponent<ServerGoldDashBlindingShieldMain>();
            AddComponent<ServerAIAttribute>(new ServerAIAttribute.InitData { // 怪物属性
                ATK = 0,
                AttackRange = 0,
                MaxHp = useMData.Hp,
            });
            AddComponent<ServerGoldDashAIHurt>();
            AddComponent<ServerGoldDashBlindingShieldMove>();
        }
    }
}