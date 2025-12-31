using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientGamePlay;
using Sofunny.BiuBiuBiu2.ServerGamePlay;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.GamePlay {
    public class GameWorld : IGameWorldManager {
        private CoreGameWorld host;
#if SERVER_LOGIC
        private ServerGameWorld server;
#endif
        private ClientGameWorld client;
        
        public void Init() {
            InitManager();
        }

        public void Clear() {
            RemoveHost();
#if SERVER_LOGIC
            RemoveServer();
#endif
            RemoveClient();
        }

        public void Update(float deltaTime) {
            PerfAnalyzerAgent.BeginSample("host.Update");
            host?.Update(deltaTime);
            PerfAnalyzerAgent.EndSample("host.Update");
#if SERVER_LOGIC
            PerfAnalyzerAgent.BeginSample("server.Update");
            server?.Update(deltaTime);
            PerfAnalyzerAgent.EndSample("server.Update");
#endif
            PerfAnalyzerAgent.BeginSample("client.Update");
            client?.Update(deltaTime);
            PerfAnalyzerAgent.EndSample("client.Update");
        }

        public void InitManager() {
            StartHost();
#if SERVER_LOGIC
            if (NetworkData.IsStartServer) {
                StartServer();
            }
#endif
            if (NetworkData.IsStartClient) {
                StartClient();
            }
            MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                loadState = StageData.LoadEnum.Game,
            });
        }

        private void StartHost() {
            host = new CoreGameWorld();
            host.Init();
        }

        private void RemoveHost() {
            host.Clear();
            host = null;
        }

#if SERVER_LOGIC
        private void StartServer() {
            server = new ServerGameWorld();
            server.Init();
        }

        private void RemoveServer() {
            server?.Clear();
            server = null;
        }
#endif

        private void StartClient() {
            client = new ClientGameWorld();
            client.Init();
        }

        private void RemoveClient() {
            client?.Clear();
            client = null;
        }
    }
}