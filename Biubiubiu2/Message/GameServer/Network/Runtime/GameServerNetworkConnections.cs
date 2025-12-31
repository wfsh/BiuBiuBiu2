using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.GameServerMessage {
    public interface IMessage {
        byte ServiceID();
        byte MessageID();
        String Identity();
        String ToString();
    }

    public interface IProtocol {
        IMessage NewRequest(byte serviceID, byte messageID);
        IMessage NewResponse(byte serviceID, byte messageID);
    }

    public class GameServerNetworkConnection {
        private GameServerNetworkChannel channel;
        private uint id;
        private uint remoteId;

        private IProtocol proto;
        private Action<IMessage> onProcess;

        private readonly Queue<byte[]> waitSendBuffers = new();

        public NetworkStatus Status { get; private set; }
        
        public bool Closed { get; private set; }

        public GameServerNetworkConnection(GameServerNetworkChannel channel, uint id, uint remoteId, IProtocol p, Action<IMessage> process) {
            this.channel = channel;
            this.id = id;
            this.remoteId = remoteId;
            this.proto = p;
            this.onProcess = process;
            Status = NetworkStatus.Disconnected;
        }
        
        public GameServerNetworkConnection(GameServerNetworkChannel channel, uint id, uint remoteId) : this(channel, id, remoteId, null, null) {
            Status = NetworkStatus.Connected;
        }


        public void Send(IMessage message) {
            try {
                int headSize = 2; // 1bit serviceID and 1bit messageID
                var protoMsg = message as Google.Protobuf.IMessage;
                byte[] buffer = new byte[headSize + protoMsg.CalculateSize()];
                Stream writer = new MemoryStream(buffer);

                // ServiceID and MessageID
                writer.WriteByte(message.ServiceID());
                writer.WriteByte(message.MessageID());

                // Marshal message
                Google.Protobuf.CodedOutputStream output = new(writer);
                protoMsg.WriteTo(output);
                output.Flush();

                if (Closed) {
                    return;
                }
                if (Status != NetworkStatus.Connected) {
                    waitSendBuffers.Enqueue(buffer);
                    return;
                }
                InternalSendAsync(buffer);
            } catch (Exception ex) {
                Close();
            }
        }

        private void InternalSendAsync(byte[] buffer) {
            var packetBuffer = new byte[buffer.Length + 8];
            using MemoryStream ms = new MemoryStream(packetBuffer);
            using BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((uint)(4 + buffer.Length));
            bw.Write(id);
            bw.Write(buffer);
            channel.SendAsync(new GameServerNetworkPacket(packetBuffer));
        }

        public void ConnectAsync() {
            Status = NetworkStatus.Connecting;
            
            var buffer = new byte[13];
            using MemoryStream ms = new MemoryStream(buffer);
            using BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((uint)9);
            bw.Write((uint)0);
            bw.Write((byte)0);
            bw.Write(remoteId);
            channel.SendAsync(new GameServerNetworkPacket(buffer));
        }

        internal void OnReceivedPacket(GameServerNetworkPacket packet) {
            IMessage message = null;
            try {
                using var ms = new MemoryStream(packet.Buffer);
                using var br = new BinaryReader(ms);

                var serviceId = br.ReadByte();
                var messageId = br.ReadByte();

                // Unmarshal message
                message = proto.NewResponse(serviceId, messageId);
                Google.Protobuf.IMessage protoMsg = message as Google.Protobuf.IMessage;

                Google.Protobuf.CodedInputStream input = new(ms);
                protoMsg.MergeFrom(input);

                if (ms.Position != ms.Length) {
                    throw new Exception($"Proto key:{message.Identity()} ---> decode buffer lenth error");
                }

                // Call process
                onProcess(message);
            } catch (Exception ex) {
                Debug.Log($"OnReceivedPacket catch error ---> {ex}");
                Close();
            }
        }

        internal void OnConnectSuccess(uint id) {
            this.id = id;
            Status = NetworkStatus.Connected;

            while (waitSendBuffers.Count > 0) {
                var buffer = waitSendBuffers.Dequeue();
                InternalSendAsync(buffer);
            }
        }

        public void Close() {
            if (Closed) {
                return;
            }
            Closed = true;
            Status = NetworkStatus.Disconnected;
            onProcess = null;
            channel.CloseConnection(id);
            channel = null;
        }
    }

}