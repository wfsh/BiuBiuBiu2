using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterComponent : ServerNetworkComponentBase {
        protected ICharacterSync characterSync {
            get;
            private set;
        }
        protected INetworkCharacter characterNetwork {
            get;
            private set;
        }
        protected S_Character_Base characterSystem {
            get;
            private set;
        }

        protected override void OnAwakeBase() {
            base.OnAwakeBase();
            characterSystem = (S_Character_Base)mySystem;
        }

        protected override void OnClearBase() {
            base.OnClearBase();
            characterSync = null;
            characterNetwork = null;
            characterSystem = null;
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
    }
}