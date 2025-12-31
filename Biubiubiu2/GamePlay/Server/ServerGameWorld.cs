using System;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGameWorld : AbsGameWorld {
        protected override void OnInit() {
            AddManager<ServerNetworkManager>();
            AddManager<ServerGPOManager>();
            AddManager<ServerSceneManager>();
            AddManager<ServerAbilityManager>();
            AddManager<ServerAIManager>();
            AddManager<ServerWeaponManager>();
            AddManager<ServerItemManager>();
            AddManager<ServerAIBehaviorManager>();
            AddManager<ServerEventDirectorManager>();
            AddManager<ServerModeManager>();
            if (ModeData.IsSausageMode()) {
                AddManager<ServerGoldDashCharacterManager>();
           		AddManager<ServerSausageAICharacterManager>();
            } else {
                AddManager<ServerCharacterManager>();
                // AddManager<ServerGrpcManager>();
#if BUILD_SERVER || UNITY_EDITOR
                // AddManager<ServerWarReportManager>();
#endif
#if !RELEASE
                AddManager<ServerShortcutToolManager>();
#endif
            }
        }
        
        protected override void OnClear() {
            base.OnClear();
        }
    }
}