using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSceneSystem : SystemBase {
        protected override void OnAwake() {
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ServerSceneGetScenePoints>();
            AddComponent<ServerSceneSerialize>();
            AddComponent<ServerSceneElement>();
            AddComponent<ServerSceneAutoGatheringElement>();
            AddComponent<ServerSceneElementPlayAbility>();
        }
    }
}