using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerEventDirectorSystem : SystemBase {
        protected override void OnAwake() {
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ServerEventDirectorGetData>();
            AddComponent<ServerEventDirectorCore>();
            //AddComponent<ServerEventActionNotifier>();
        }
    }
}