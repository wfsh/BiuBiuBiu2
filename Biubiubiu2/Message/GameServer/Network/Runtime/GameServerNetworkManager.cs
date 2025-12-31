using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor;

namespace Sofunny.BiuBiuBiu2.GameServerMessage {

    public partial class GameServerNetworkManager : MonoBehaviour {
        private readonly Dictionary<string, INetworkChannel> channels = new Dictionary<string, INetworkChannel>();

        void Awake() {
            instance = this;
        }

        void OnDisable() {
            instance = null;
        }

        void Update() {
            if (channels.Count == 0) {
                return;
            }
            foreach (var channel in channels.Values) {
                channel.Update();
            }
        }

        public async Task<NetworkError> ConnectGameServer(string url, string fastwayAddress,
            Action<NetworkStatus> onConnect) {
            if (!channels.TryGetValue(GAMESERVER_CHANNEL, out var channel)) {
                channel = new GameServerNetworkChannel();
                channel.SetConnectTimeout(2000)
                    .SetNetworkReceivePacket(new GameServerNetworkReceivePacket())
                    .SetNetworkSendPacket(new GameServerNetworkSendPacket())
                    .SetKeepNetworkAlive(new GameServerHeartPacket(), 5000, 3000)
                    .SetAutoReconnect(5, new GameServerSendCachePacket(8192));
                channel.SetOnConnected(onConnect);
                channels.Add(GAMESERVER_CHANNEL, channel);
            }
            var gameServerChannel = channel as GameServerNetworkChannel;
            gameServerChannel.SetFastwayAddress(fastwayAddress);
            var error = await channel.ConnectAsync(url);
            return error;
        }

        public NetworkStatus GetNetworkStatus(string channelName) {
            if (!channels.TryGetValue(channelName, out var channel)) {
                return NetworkStatus.Disconnected;
            }
            return channel.Status;
        }

        public NetworkError GetNetworkError(string channelName) {
            if (!channels.TryGetValue(channelName, out var channel)) {
                return NetworkError.None;
            }
            return channel.NetworkError;
        }

        public GameServerNetworkConnection Dail(uint remoteID, IProtocol p, Action<IMessage> process) {
            if (!channels.TryGetValue(GAMESERVER_CHANNEL, out var channel)) {
                throw new Exception("Network channel not created");
            }
            var gameServerChannel = channel as GameServerNetworkChannel;
            var conn = gameServerChannel.CreateNetworkConnection(remoteID, p, process);
            return conn;
        }

        public void Close(string channelName) {
            if (channels.TryGetValue(channelName, out var channel)) {
                channel.Close();
                channels.Remove(channelName);
            }
        }

        public void CloseAll() {
            foreach (var channel in channels.Values) {
                channel.Close();
            }
            channels.Clear();
        }
    }

}