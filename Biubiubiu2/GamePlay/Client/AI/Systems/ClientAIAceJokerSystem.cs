using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIAceJokerSystem : C_AI_Base {
        private GPOM_AceJoker useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_AceJoker)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            iEntity.SetPoint(startPoint);
            iEntity.SetRota(startRot);
            CreateEntity(AISkinSign);
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<ClientAIAttribute>();
            AddComponent<ClientAceJokerPreAnimLoadEffect>();
            AddComponent<ClientAIAnim>(new ClientAIAnim.InitData {
                ConfigSign = AIAnimConfig.GoldDash_BOSSAceJoker,
                ChangeAssetUrl = "BossJoker",
                ToAssetUrl = useMData.GetSign(),
            });
        }
    }
}