using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGameWorld : AbsGameWorld {
        protected override void OnInit() {
            AddManager<ClientNetworkManager>();
            AddManager<ClientGPOManager>();
            AddManager<ClientSceneManager>();
            AddManager<ClientWeaponManager>();
            AddManager<ClientCharacterManager>();
            AddManager<ClientAbilityManager>();
            AddManager<ClientAIManager>();
            AddManager<ClientItemManager>();
            AddManager<ClientEventDirectorManager>();
            if (ModeData.PlayMode != ModeData.ModeEnum.SausageGoldDash) {
                // AddManager<ClientGameTestManager>();
                AddManager<ClientModeManager>();
                // AddManager<CameraManager>();
            }
            if (WarReportData.IsStartWarReport()) {
                // AddManager<ClientWarReportManager>();
            }
        }
        protected override void OnClear() {
            base.OnClear();
        }
    }
}