using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.GameServerMessage {

    public interface INetworkChannel {
        NetworkStatus Status { get; }
        NetworkError NetworkError { get; }
        bool Disposed { get; }
        INetworkChannel SetConnectTimeout(int milliseconds);
        INetworkChannel SetReadTimeout(int milliseconds);
        INetworkChannel SetWriteTimeout(int milliseconds);
        INetworkChannel SetOnConnected(System.Action<NetworkStatus> onConnected);
        INetworkChannel SetNetworkReceivePacket(INetworkReceivePacket receivePacket);
        INetworkChannel SetNetworkSendPacket(INetworkSendPacket sendPacket);
        INetworkChannel SetKeepNetworkAlive(INetworkHeartPacket heartPacket, double sendHeartPacketInterval, double heartPacketTimeout);
        INetworkChannel SetAutoReconnect(int reconnectTimesLimit, INetworkSendCachePacket packet);
        Task<NetworkError> ConnectAsync(string url);
        void SendAsync(INetworkPacket packet);
        void Update();
        void Close();
    }

}