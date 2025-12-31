using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class MirrorConnectSystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            if (NetworkData.IsStartServer) {
                AddComponent<ServerSpawnPrefabs>();
                AddComponent<MirrorConnect>();
            } else {
                AddComponent<MirrorClientConnect>();
            }
            AddComponent<MirrorNetworkInvoke>();
            MsgRegister.Register<M_Network.NetworkConfigInit>(OnNetworkConfigInitCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Network.NetworkConfigInit>(OnNetworkConfigInitCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            if (NetworkData.Config == null) {
                Debug.Log("NetworkData.Config 尚未初始化，等待初始化");
                return;
            }
            if (Application.platform == RuntimePlatform.WebGLPlayer) {
                if (NetworkData.IsStartServer) {
                    NetworkData.Config.IsKCP = true;
                }
            }
            LoadNetworkEntity();
        }

        private void OnNetworkConfigInitCallBack(M_Network.NetworkConfigInit ent) {
            LoadNetworkEntity();
        }

        private void LoadNetworkEntity() {
            MsgRegister.Unregister<M_Network.NetworkConfigInit>(OnNetworkConfigInitCallBack);
            if (NetworkData.Config == null) {
                Debug.LogError("NetworkData.Config 尚未初始化，错误");
                return;
            }
            if (NetworkData.Config.IsKCP) {
                if (NetworkData.IsStartServer) {
                    Debug.Log("加载 NetworkEntity MirrorEntityThreadedKcp");
                    CreateEntityObj( "Network/Mirror/MirrorEntityThreadedKcp", StageData.GameWorldLayerType.Base);
                } else {
#if UNITY_WEBGL || UNITY_EDITOR
                    Debug.Log("加载 NetworkEntity MirrorEntityKcp");
                    CreateEntityObj( "Network/Mirror/MirrorEntityKcp", StageData.GameWorldLayerType.Base);
#else 
                    Debug.Log("加载 NetworkEntity MirrorEntityThreadedKcp");
                    CreateEntityObj( "Network/Mirror/MirrorEntityThreadedKcp", StageData.GameWorldLayerType.Base);
#endif
                }
            } else {
                Debug.Log("加载 NetworkEntity MirrorEntityWeb");
                CreateEntityObj( "Network/Mirror/MirrorEntityWeb", StageData.GameWorldLayerType.Base);
            }
        }
    }
}
