using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGrpcManager : ManagerBase {
        private ServerGrpcSystem grpcSystem;
        protected override void OnAwake() {
            base.OnAwake();
            InitGrpc();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            grpcSystem.Clear();
            grpcSystem = null;
        }

        private void InitGrpc() {
            grpcSystem = AddSystem(delegate(ServerGrpcSystem system) {
            });
        }
    }
}