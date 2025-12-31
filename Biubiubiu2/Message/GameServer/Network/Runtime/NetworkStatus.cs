using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.GameServerMessage {

    public enum NetworkStatus {
        Disconnected,
        Connecting,
        ConnectFailed,
        Connected,
        Reconnecting,
        ReconnectFailed
    }

    public enum NetworkError {
        None,
        InvalidOperation,
        Timeout,
        Exception,
        ResponseError,
        EndOfStream,
        ReconnectDenied,
    }

}