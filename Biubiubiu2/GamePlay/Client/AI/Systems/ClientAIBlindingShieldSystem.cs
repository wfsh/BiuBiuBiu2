namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIBlindingShieldSystem : C_AI_Base {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            iEntity.SetPoint(startPoint);
        }
        
        protected override void AddComponents() {
            AddComponent<ClientNetworkTransform>();
            AddComponent<ClientAIAttribute>();
            AddComponent<ClientAIRemove>();
        }
    }
}