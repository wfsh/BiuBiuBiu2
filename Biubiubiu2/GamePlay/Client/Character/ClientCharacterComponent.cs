using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterComponent : ComponentBase {
        protected ICharacterSync characterSync {
            get;
            private set;
        }
        protected INetworkCharacter characterNetwork {
            get;
            private set;
        }
        
        protected override void OnClearBase() {
            base.OnClearBase();
            characterSync = null;
            characterNetwork = null;
        }

        protected override void OnSetNetworkBase() {
            base.OnSetNetworkBase();
            if (networkBase == null) {
                return;
            }
            characterSync = (ICharacterSync)networkBase.GetNetworkSync();
            characterNetwork = (INetworkCharacter)networkBase;
            characterNetwork.SetCharacterReady(true);
        }

        protected void Cmd(ICmd proto) {
            if (ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver) {
                return;
            }
            characterNetwork?.Cmd(proto);
        }
    }
}