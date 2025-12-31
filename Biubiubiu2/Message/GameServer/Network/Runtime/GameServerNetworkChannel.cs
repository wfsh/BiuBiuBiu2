using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using Snet;

namespace Sofunny.BiuBiuBiu2.GameServerMessage {

    public sealed class GameServerNetworkChannel : WebSocketClientNetworkChannel { // WebSocketClientNetworkChannel TcpClientNetworkChannel
        private const byte NEW_CONNECT = 0x00;
        private const byte RE_CONNECT = 0xFF;

        private string fastwayAddress;
        private readonly byte[] keyBuffer = new byte[8];

        private byte[] reconnectRequestBuffer = null;

        private ulong id;

        private readonly Dictionary<uint, List<GameServerNetworkConnection>> waitConnectConnections = new();
        private readonly Dictionary<uint, GameServerNetworkConnection> connections = new();

        public INetworkChannel SetFastwayAddress(string fastwayAddress) {
            this.fastwayAddress = fastwayAddress;
            return this;
        }

        public override async Task<NetworkError> ConnectAsync(string url) {
            if (Status != NetworkStatus.Disconnected) {
                return NetworkError.InvalidOperation;
            }

            Status = NetworkStatus.Connecting;

            try {
                var error = await ConnectServerAsync(url);
                if (error != NetworkError.None) {
                    OnConnectFailed(error);
                    return error;
                }

                error = await ConnectFastwayAsync();
                if (error != NetworkError.None) {
                    OnConnectFailed(error);
                    return error;
                }

                error = await SetPublicSecretKey();
                if (error != NetworkError.None) {
                    OnConnectFailed(error);
                    return error;
                }

                OnConnectSuccess();
                return NetworkError.None;
            } catch (Exception e) {
                OnConnectFailed(NetworkError.Exception);
                return NetworkError.Exception;
            }
        }

        private async Task<NetworkError> ConnectFastwayAsync() {
            var bytes = System.Text.Encoding.UTF8.GetBytes(fastwayAddress);
            await webSocket.WriteAsync(bytes, 0, bytes.Length);
            var response = new byte[3];
            Debug.Log("等待网关链接");
            await webSocket.ReceiveAsync(response, 0, 3);
            Debug.Log("网关链接成功");
            var code = System.Text.Encoding.UTF8.GetString(response);
            var result = Convert.ToInt32(code);
            if (result != 200) {
                Debug.Log($"网关链接失败：{result}");
                return NetworkError.ResponseError;
            }

            return NetworkError.None;
        }

        private async Task<NetworkError> SetPublicSecretKey() {
            var cmd = new byte[1];
            var request = new byte[24];
            var response = request;

            cmd[0] = NEW_CONNECT;

            var dh64 = new DH64();
            dh64.KeyPair(out var privateKey, out var publicKey);

            unsafe {
                fixed (byte* b = request) {
                    *(ulong*)b = publicKey;
                }
            }

            await webSocket.WriteAsync(cmd, 0, cmd.Length);
            await webSocket.WriteAsync(request, 0, 8);

            for (int n = 24; n > 0;) {
                int x = await webSocket.ReceiveAsync(response, 24 - n, n);
                if (x == 0) {
                    return NetworkError.EndOfStream;
                }
                n -= x;
            }

            using var ms1 = new MemoryStream(response, 0, 24);
            using var br1 = new BinaryReader(ms1);
            var serverPublicKey = br1.ReadUInt64();
            var secret = dh64.Secret(privateKey, serverPublicKey);

            using MemoryStream ms2 = new MemoryStream(keyBuffer);
            using BinaryWriter bw2 = new BinaryWriter(ms2);
            bw2.Write(secret);

            var readCipher = new RC4Cipher(keyBuffer);
            var writeCipher = new RC4Cipher(keyBuffer);
            readCipher.XORKeyStream(response, 8, response, 8, 8);

            id = br1.ReadUInt64();

            var receivePacket = ReceivePacket as GameServerNetworkReceivePacket;
            receivePacket.SetReadCipher(readCipher);

            var sendPacket = SendPacket as GameServerNetworkSendPacket;
            sendPacket.SetWriteCipher(writeCipher);

            using MemoryStream ms3 = new MemoryStream(request, 0, 16);
            using BinaryWriter bw3 = new BinaryWriter(ms3);
            bw3.Write(response, 16, 8);
            bw3.Write(keyBuffer);
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(request, 0, 16);
            Buffer.BlockCopy(hash, 0, request, 0, hash.Length);
            await webSocket.WriteAsync(request, 0, 16);

            return NetworkError.None;
        }

        protected override void StartReconnect() {
            reconnectTimes = 0;
            reconnectRequestBuffer = null;
            base.StartReconnect();
        }

        protected override async Task<NetworkError> ReconnectAsync() {
            try {
                if (reconnectTimes > 1) {
                    await Task.Delay(3000);
                }

                reconnectTimes++;

                var error = await ConnectServerAsync(Url);
                if (error != NetworkError.None) {
                    OnReconnectFailed(error);
                    return error;
                }
                
                error = await ConnectFastwayAsync();
                if (error != NetworkError.None) {
                    OnReconnectFailed(error);
                    return error;
                }
                
                var tuple = await SyncReceivedSendBytes();
                if (tuple.Item1 != NetworkError.None) {
                    OnReconnectFailed(error);
                    return error;
                }
                
                error = await SendLossPacketAsync(tuple.Item3);
                if (error != NetworkError.None) {
                    OnReconnectFailed(error);
                    return error;
                }

                OnReconnectSuccess();
                return NetworkError.None;
            } catch (Exception e) {
                OnReconnectFailed(NetworkError.Exception);
                return NetworkError.Exception;
            }
        }

        private async Task<(NetworkError, ulong, ulong)> SyncReceivedSendBytes() {
            var cmd = new byte[] { RE_CONNECT };
            await webSocket.WriteAsync(cmd, 0, cmd.Length);

            var md5 = MD5.Create();
            byte[] hash = null;
            if (reconnectRequestBuffer == null) {
                reconnectRequestBuffer = new byte[40];
                using var ms1 = new MemoryStream(reconnectRequestBuffer);
                using var bw1 = new BinaryWriter(ms1);
                bw1.Write(id);
                bw1.Write(totalSentBytes);
                bw1.Write(totalReceivedBytes);
                bw1.Write(keyBuffer);

                hash = md5.ComputeHash(reconnectRequestBuffer, 0, 32);
                Buffer.BlockCopy(hash, 0, reconnectRequestBuffer, 24, hash.Length);
            }

            await webSocket.WriteAsync(reconnectRequestBuffer, 0, reconnectRequestBuffer.Length);

            var response = new byte[24];
            for (int n = response.Length; n > 0;) {
                int x = await webSocket.ReceiveAsync(response, response.Length - n, n);
                if (x == 0) {
                    throw new EndOfStreamException();
                }
                n -= x;
            }

            using var ms2 = new MemoryStream(response);
            using var br2 = new BinaryReader(ms2);
            var writeCount = br2.ReadUInt64();
            var readCount = br2.ReadUInt64();
            var challengeCode = br2.ReadUInt64();
            if (writeCount == 0 && readCount == 0 && challengeCode == 0) {
                return (NetworkError.ReconnectDenied, 0, 0);
            }

            using MemoryStream ms3 = new MemoryStream(reconnectRequestBuffer, 0, 16);
            using BinaryWriter bw3 = new BinaryWriter(ms3);
            bw3.Write(response, 16, 8);
            bw3.Write(keyBuffer);
            hash = md5.ComputeHash(reconnectRequestBuffer, 0, 16);
            Buffer.BlockCopy(hash, 0, reconnectRequestBuffer, 0, hash.Length);
            await webSocket.WriteAsync(reconnectRequestBuffer, 0, 16);

            if (writeCount < totalReceivedBytes) {
                return (NetworkError.ReconnectDenied, 0, 0);
            }

            if (totalSentBytes < readCount) {
                return (NetworkError.ReconnectDenied, 0, 0);
            }

            return (NetworkError.None, writeCount, readCount);
        }

        private async Task<NetworkError> SendLossPacketAsync(ulong serverReceivedBytes) {
            if (totalSentBytes != serverReceivedBytes) {
                var resendBytes = (int)(totalSentBytes - serverReceivedBytes);
                if (resendBytes < 0 || resendBytes > SendCachePacket.Position) {
                    return NetworkError.Exception;
                }
                await webSocket?.WriteAsync(SendCachePacket.Buffer, SendCachePacket.Position - resendBytes, resendBytes);
            }
            return NetworkError.None;
        }

        protected override void OnReceivedPacket(INetworkPacket receivePacket) {
            var packet = receivePacket as GameServerNetworkPacket;
            if (packet.connID != 0) {
                if (!connections.TryGetValue(packet.connID, out var connection)) {
                    CloseConnection(packet.connID);
                } else {
                    connection.OnReceivedPacket(packet);
                }
                return;
            }
            switch (packet.Buffer[0]) {
                case 1:
                    OnConnectionConnectSucess(packet.Buffer);
                    break;
                case 2:
                    OnConnectionCreate(packet.Buffer);
                    break;
                case 3:
                    OnConnectionRefuse(packet.Buffer);
                    break;
                case 4:
                    OnConnectionClose(packet.Buffer);
                    break;
                case 5:
                    OnHeartPacketCallBack();
                    break;
            }
        }

        private void OnConnectionConnectSucess(byte[] buffer) {
            using MemoryStream ms = new MemoryStream(buffer, 1, 8);
            using BinaryReader br = new BinaryReader(ms);
            var connID = br.ReadUInt32();
            var remoteID = br.ReadUInt32();

            if (!this.waitConnectConnections.TryGetValue(remoteID, out var waitConnections) || waitConnections.Count == 0) {
                CloseConnection(connID);
                return;
            }

            var connection = waitConnections[0];
            waitConnections.RemoveAt(0);
            connections.Add(connID, connection);
            connection.OnConnectSuccess(connID);
        }

        private void OnConnectionCreate(byte[] buffer) {
            using MemoryStream ms = new MemoryStream(buffer, 1, 8);
            using BinaryReader br = new BinaryReader(ms);
            var connID = br.ReadUInt32();
            var remoteID = br.ReadUInt32();

            var conn = new GameServerNetworkConnection(this, connID, remoteID);
            connections.Add(connID, conn);
        }

        private void OnConnectionRefuse(byte[] buffer) {
            using MemoryStream ms = new MemoryStream(buffer, 1, 4);
            using BinaryReader br = new BinaryReader(ms);
            var remoteID = br.ReadUInt32();

            if (waitConnectConnections.TryGetValue(remoteID, out var waitConnections) && waitConnections.Count > 0) {
                var connection = waitConnections[0];
                waitConnections.RemoveAt(0);
                connection.Close();
            }
        }

        private void OnConnectionClose(byte[] buffer) {
            using MemoryStream ms = new MemoryStream(buffer, 1, 4);
            using BinaryReader br = new BinaryReader(ms);
            var connID = br.ReadUInt32();

            if (this.connections.TryGetValue(connID, out var connection)) {
                connection.Close();
            }
        }

        public GameServerNetworkConnection CreateNetworkConnection(uint remoteID, IProtocol p, Action<IMessage> process) {
            if (!waitConnectConnections.TryGetValue(remoteID, out var connections)) {
                connections = new List<GameServerNetworkConnection>();
                waitConnectConnections.Add(remoteID, connections);
            }
            var connection = new GameServerNetworkConnection(this, 0, remoteID, p, process);
            connection.ConnectAsync();
            connections.Add(connection);
            return connection;
        }

        public void CloseConnection(uint id) {
            if (Disposed) {
                return;
            }
            if (id != 0) {
                if (connections.TryGetValue(id, out var connection)) {
                    connection.Close();
                    connections.Remove(id);
                }
            }

            var buffer = new byte[13];
            using MemoryStream ms = new MemoryStream(buffer);
            using BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((uint)9);
            bw.Write((uint)0);
            bw.Write((byte)4);
            bw.Write(id);
            SendAsync(new GameServerNetworkPacket(buffer));
        }

        public override void Close() {
            base.Close();
            foreach (var connection in connections.Values) {
                connection.Close();
            }
            connections.Clear();
            foreach (var connections in waitConnectConnections.Values) {
                foreach (var connection in connections) {
                    connection.Close();
                }
                connections.Clear();
            }
            waitConnectConnections.Clear();
        }
    }

}