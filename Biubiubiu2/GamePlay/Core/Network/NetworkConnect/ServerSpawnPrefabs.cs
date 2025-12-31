using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class ServerSpawnPrefabs : ComponentBase {
        private List<GameObject> spawnPrefabs;

        protected override void OnAwake() {
            MsgRegister.Register<M_Network.SetSpawnPrefabs>(OnSetSpawnPrefabsCallBack);
            MsgRegister.Register<M_Network.Spawn>(OnSpawnCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Network.SetSpawnPrefabs>(OnSetSpawnPrefabsCallBack);
            MsgRegister.Unregister<M_Network.Spawn>(OnSpawnCallBack);
        }

        public void OnSetSpawnPrefabsCallBack(M_Network.SetSpawnPrefabs ent) {
            SetSpawnPrefabs(ent.SpawnPrefabs);
        }
        
        private void SetSpawnPrefabs(List<GameObject> list) {
            spawnPrefabs = list;
        }

        public void OnSpawnCallBack(M_Network.Spawn ent) {
            var obj = GetSpawnPrefab(ent.Sign);
            if (obj) {
                var gameObj = GameObject.Instantiate(obj);
                ent.CallBack.Invoke(gameObj);
                Mirror.NetworkServer.Spawn(gameObj);
            } else {
                Debug.LogError("没有找到可以 Spawn 的网络组件:" + ent.Sign);
            }
        }

        public GameObject GetSpawnPrefab(string sign) {
            for (int i = 0; i < spawnPrefabs.Count; i++) {
                var obj = spawnPrefabs[i];
                if (obj.name == sign) {
                    return obj;
                }
            }
            return null;
        }
    }
}