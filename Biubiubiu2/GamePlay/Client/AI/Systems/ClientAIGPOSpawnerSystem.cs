using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIGPOSpawnerSystem : C_AI_Base {
        private GPOM_Gpospawner useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_Gpospawner)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            iEntity.SetPoint(startPoint);
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<ClientGPOSpawnerWaveMainLoop>(new ClientGPOSpawnerWaveMainLoop.InitData {
                configSign = useMData.GpoSoConfig
            });
        }
    }
}