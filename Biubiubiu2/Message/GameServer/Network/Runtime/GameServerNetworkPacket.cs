using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Snet;

namespace Sofunny.BiuBiuBiu2.GameServerMessage {

    public class GameServerNetworkReceivePacket : INetworkReceivePacket {
        private const int HEADER_LENGTH = 8;

        public byte[] Buffer { get; private set; }
        public int Length {
            get {
                return Buffer.Length;
            }
        }

        public int ReceivedLength { get; private set; }

        private RC4Cipher cipher;
        
        public Queue<INetworkPacket> NetworkPackets { get; private set; }

        private GameServerNetworkPacket packet = null;

        private bool inReadingBody = false;

        public GameServerNetworkReceivePacket() {
            Buffer = new byte[1024 * 64];
            NetworkPackets = new Queue<INetworkPacket>();
            inReadingBody = false;
        }

        public void SetReadCipher(RC4Cipher cipher) {
            this.cipher = cipher;
        }

        public void ProcessPacket(int receiveLength) {
            ProcessDecryptPacket(receiveLength);
            ReceivedLength += receiveLength;
            var position = 0;
            var remainingLength = ReceivedLength;
            while (remainingLength >= HEADER_LENGTH || (packet != null && remainingLength > packet.Length - packet.Position)) {
                if (packet == null) {
                    packet = ProcessPacketHead(position);
                    position += HEADER_LENGTH;
                }
                position += ProcessPacketBody(position);
                if (packet.EndOfStream) {
                    NetworkPackets.Enqueue(packet);
                    packet = null;
                }
                remainingLength = ReceivedLength - position;
            }
            ResetReceivedLength(position, remainingLength);
        }
        
        private void ProcessDecryptPacket(int receiveLength) {
            cipher?.XORKeyStream(Buffer, ReceivedLength, Buffer, ReceivedLength, receiveLength);
        }

        private GameServerNetworkPacket ProcessPacketHead(int offset) {
            using MemoryStream ms = new MemoryStream(Buffer);
            using BinaryReader br = new BinaryReader(ms);
            var length = (int)BitConverter.ToUInt32(new ReadOnlySpan<byte>(Buffer, offset, 4));
            var connID = BitConverter.ToUInt32(new ReadOnlySpan<byte>(Buffer, offset + 4, 4));
            return new GameServerNetworkPacket(connID, length - 4);
        }

        private int ProcessPacketBody(int offset) {
            var length = Mathf.Min(packet.Length - packet.Position, ReceivedLength - offset);
            packet.Write(Buffer, offset, length);
            return length;
        }

        private unsafe void ResetReceivedLength(int position, int remainingLength) {
            for (int i = 0; i < remainingLength; i++) {
                Buffer[i] = Buffer[position + i];
            }
            ReceivedLength = remainingLength;
        }
    }

    public class GameServerNetworkSendPacket : INetworkSendPacket {
        public byte[] Buffer { get; }

        public int Length {
            get {
                return Buffer.Length;
            }
        }

        public int Position { get; private set; }
        
        private RC4Cipher cipher;

        private readonly Queue<GameServerNetworkPacket> waitSendPackets = new Queue<GameServerNetworkPacket>();

        public GameServerNetworkSendPacket() {
            Buffer = new byte[1024 * 64];
            Position = 0;
        }

        public void Enqueue(INetworkPacket sendPacket) {
            var packet = sendPacket as GameServerNetworkPacket;
            cipher?.XORKeyStream(packet.Buffer, 0, packet.Buffer, 0, packet.Length);
            waitSendPackets.Enqueue(packet);
        }
        
        public void SetWriteCipher(RC4Cipher cipher) {
            this.cipher = cipher;
        }

        public unsafe void ProcessPacket() {
            while (Position < Length && waitSendPackets.Count > 0) {
                var packet = waitSendPackets.Peek();
                var span = packet.Read(Length - Position);
                if (packet.EndOfStream) {
                    waitSendPackets.Dequeue();
                }
                fixed (byte* s = span) {
                    UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref Buffer[Position]), s, span.Length);
                }
                Position += span.Length;
            }
        }

        public void ResetPacketBuffer() {
            Position = 0;
        }
    }

    public class GameServerHeartPacket : INetworkHeartPacket {
        public INetworkPacket GetHeartPacket() {
            var buffer = new byte[9];
            using var ms = new MemoryStream(buffer);
            using var bw = new BinaryWriter(ms);
            bw.Write((uint)5);
            bw.Write((uint)0);
            bw.Write((byte)5);
            return new GameServerNetworkPacket(buffer);
        }
    }

    public class GameServerSendCachePacket : INetworkSendCachePacket {
        private byte[] buffer;
        private int position;

        public int Position {
            get {
                return position;
            }
        }

        private int RemainingLength {
            get {
                return buffer.Length - Position;
            }
        }

        public byte[] Buffer {
            get {
                return buffer;
            }
        }

        public GameServerSendCachePacket(int size) {
            buffer = new byte[size];
            position = 0;
        }

        public unsafe void Write(byte[] buffer, int offset, int size) {
            if (size > this.buffer.Length) {
                var drop = size - this.buffer.Length;
                UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref this.buffer[0]), UnsafeUtility.AddressOf(ref buffer[offset + drop]), size - drop);
                position = this.buffer.Length;
            } else {
                if (size > RemainingLength) {
                    var drop = size - RemainingLength;
                    UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref this.buffer[position - drop]), UnsafeUtility.AddressOf(ref buffer[offset]), size);
                    position = this.buffer.Length;
                } else {
                    UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref this.buffer[position]), UnsafeUtility.AddressOf(ref buffer[offset]), size);
                    position += size;
                }
            }
        }
    }

    public class GameServerNetworkPacket : INetworkPacket {
        public readonly uint connID = 0;
        
        public byte[] Buffer { get; private set; }

        public int Length {
            get {
                return Buffer.Length;
            }
        }

        public int Position { get; private set; }

        public bool EndOfStream {
            get {
                return Position == Length;
            }
        }

        public GameServerNetworkPacket(uint connID, int length) {
            this.connID = connID;
            this.Buffer = new byte[length];
            Position = 0;
        }
        
        public GameServerNetworkPacket(byte[] buffer) {
            this.Buffer = buffer;
            Position = 0;
        }

        public unsafe void Write(byte[] buffer, int offset, int length) {
            UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref Buffer[Position]), UnsafeUtility.AddressOf(ref buffer[offset]), length);
            Position += length;
        }

        public ReadOnlySpan<byte> Read(int length) {
            var readed = Mathf.Min(length, Length - Position);
            var span = new ReadOnlySpan<byte>(Buffer, Position, readed);
            Position += readed;
            return span;
        }
    }

}