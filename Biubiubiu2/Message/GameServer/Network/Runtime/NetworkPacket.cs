using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.GameServerMessage {

    public interface INetworkPacket {
    }

    public interface INetworkReceivePacket {
        byte[] Buffer { get; }

        int Length { get; }

        int ReceivedLength { get; }

        Queue<INetworkPacket> NetworkPackets { get; }

        void ProcessPacket(int readed);
    }

    public interface INetworkSendPacket {
        byte[] Buffer { get; }
        int Length { get; }
        int Position { get; }

        void Enqueue(INetworkPacket sendPacket);
        
        void ProcessPacket();

        void ResetPacketBuffer();
    }

    public interface INetworkHeartPacket {
        INetworkPacket GetHeartPacket();
    }

    public interface INetworkSendCachePacket {
        byte[] Buffer { get; }
        int Position { get; }
        void Write(byte[] buffer, int offset, int size);
    }

}