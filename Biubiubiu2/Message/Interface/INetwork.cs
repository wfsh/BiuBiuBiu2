using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Message {
    public interface INetwork {
        bool IsDestroy();
        bool IsOnline();
        bool IsIgnoreSyncDistance();// 忽略区域剔除进行同步
        void SetParent(StageData.GameWorldLayerType layer);
        void SetName(string name);
        string GetName();
        void Rpc(IRpc doc);
        void Rpc(int connId, IRpc doc);
        void TargetRpc(INetwork conn, ITargetRpc doc);
        void TargetRpcList(List<INetworkCharacter> conn, IProto_Doc doc);
        void TargetRpc(INetwork conn, int connId, ITargetRpc doc);
        void TargetRpc(INetwork conn, int connId, IRpc doc);
        void TargetRpcList(List<INetworkCharacter> conn, int connId, IProto_Doc doc);
        int ConnId();
        byte GetNetworkId();
        void ConnectionToClientDisconnect();
        INetworkSync GetNetworkSync();
        void AddProtoCallBack(string id, Action<INetwork, IProto_Doc> action);
        void RemoveProtoCallBack(string id, Action<INetwork, IProto_Doc> action);
        void AddProtoCallBack(int connId, string id, Action<INetwork, IProto_Doc> action);
        void RemoveProtoCallBack(int connId, string id, Action<INetwork, IProto_Doc> action);
        public bool HasObservers(int connectionId);
        public void AddObserverNetwork(Action<INetwork> action);
        public void RemoveObserverNetwork(Action<INetwork> action);
    }

    public interface INetworkCharacter : INetwork {
        void Cmd(ICmd proto);
        void SetPoint(Vector3 point);
        void SetRota(Quaternion rota);
        Vector3 GetPoint();
        Quaternion GetRota();
        bool IsLocalPlayer();
        void SetCharacterReady(bool ready);
        bool IsCharacterReady();
        void SetInterpolatePositionState(bool state);
    }
    public interface INetworkSync {
        void SetNetwork(INetwork network);
    }

    public interface IWorldSync : INetworkSync {
    }

    public interface IClientWorldNetwork {
        void SetIsOnline(bool isOnline);
        int GetConnID();
    }

    public interface IServerWorldNetwork {
        void SetIsOnline(bool isOnline);
        int GetConnID();
    }

    public interface IProto_Doc {
        string GetID();
        int GetChannel();
        void Serialize(ByteBuffer buffer);
        void UnSerialize(ByteBuffer buffer);
    }

    public interface IRpc : IProto_Doc {
    }

    public interface ITargetRpc : IProto_Doc {
    }

    public interface ICmd : IProto_Doc {
    }
    public interface IAbilityABCreateRpc : IRpc {
        ushort GetConfigID();
        byte GetRowID();
    }
    public interface IAbilityAECreateRpc : IRpc {
        ushort GetConfigID();
        byte GetRowID();
    }
}