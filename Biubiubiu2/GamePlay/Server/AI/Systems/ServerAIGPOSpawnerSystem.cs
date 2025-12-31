using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIGPOSpawnerSystem : S_AI_Base {
        private GPOM_Gpospawner useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_Gpospawner)MData;
            AddComponents();
        }

        protected override void AddComponents() {
            AddComponent<ServerGPODropItem>();
            AddComponent<ServerGPOIsGodMode>();
            AddComponent<ServerGPOSpawnerSpawnLockGpo>(new ServerGPOSpawnerSpawnLockGpo.InitData {
                SpawnerGpoSign = useMData.TargetGpoSign,
                MaxHp = useMData.Hp
            });
            AddComponent<ServerGPOSpawnerWaveMainLoop>(new ServerGPOSpawnerWaveMainLoop.InitData {
                configSign = useMData.GpoSoConfig
            });
        }
    }
}