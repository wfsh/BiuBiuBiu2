using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class MirrorManager : NetworkManager {
        public event Action<NetworkConnectionToClient> OnServerConnectEvent;
        public event Action<NetworkConnectionToClient> OnServerDisconnectEvent;
        public event Action OnClientConnectEvent;
        public event Action OnClientDisconnectEvent;

        public override void OnServerConnect(NetworkConnectionToClient connection) {
            base.OnServerConnect(connection);
            OnServerConnectEvent?.Invoke(connection);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient connection) {
            base.OnServerDisconnect(connection);
            OnServerDisconnectEvent?.Invoke(connection);
        }

        public override void OnClientConnect() {
            base.OnClientConnect();
            OnClientConnectEvent?.Invoke();
        }

        public override void OnClientDisconnect() {
            base.OnClientDisconnect();
            OnClientDisconnectEvent?.Invoke();
        }
    }
}
