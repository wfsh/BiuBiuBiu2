using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterWirebug : ServerCharacterComponent {
        private const float WirebugCDTime = 5.0f; // 翔虫 CD 时间
        private const float WirebugCount = 2.0f; // 翔虫数量
        private List<float> wirebugList = new List<float>();
        private CharacterData.WirebugType wirebugType = CharacterData.WirebugType.None;

        protected override void OnAwake() {
            InitWirebug();
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        private void InitWirebug() {
            for (int i = 0; i < WirebugCount; i++) {
                wirebugList.Add(0f);
            }
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            RemoveProtoCallBack(Proto_Wirebug.Cmd_WirebugState.ID, OnUseWirebugCallBack);
            RemoveProtoCallBack(Proto_Wirebug.Cmd_CreateWirebug.ID, OnCreateWirebugCallBack);
            base.OnClear();
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Wirebug.Cmd_WirebugState.ID, OnUseWirebugCallBack);
            AddProtoCallBack(Proto_Wirebug.Cmd_CreateWirebug.ID, OnCreateWirebugCallBack);
            TargetRpc(networkBase, new Proto_Wirebug.TargetRpc_MaxUseWirebugCD() {
                maxCDTime = WirebugCDTime,
            });
            TargetRPCUseWirebug();
        }

        private void TargetRPCUseWirebug() {
            TargetRpc(networkBase, new Proto_Wirebug.TargetRpc_UseWirebug {
                cdTimeList = wirebugList,
            });
        }

        private void OnUpdate(float delta) {
            for (int i = 0; i < wirebugList.Count; i++) {
                var cdTime = wirebugList[i];
                if (cdTime > 0f) {
                    cdTime -= delta;
                    wirebugList[i] = cdTime;
                    if (cdTime < 0f) {
                        TargetRPCUseWirebug();
                    }
                }
            }
        }

        private void OnUseWirebugCallBack(INetwork network, IProto_Doc protoDoc) {
            var data = (Proto_Wirebug.Cmd_WirebugState)protoDoc;
            wirebugType = (CharacterData.WirebugType)data.wirebugType;
            var isTrue = true;
            switch (wirebugType) {
                case CharacterData.WirebugType.Start:
                    isTrue = StartState();
                    break;
            }
            if (isTrue) {
                Rpc(new Proto_Wirebug.Rpc_WirebugState {
                    wirebugType = data.wirebugType
                });
            }
        }

        private void OnCreateWirebugCallBack(INetwork network, IProto_Doc protoDoc) {
            var data = (Proto_Wirebug.Cmd_CreateWirebug)protoDoc;
            Rpc(new Proto_Wirebug.Rpc_CreateWirebug {
                targetPoint = data.targetPoint, Index = data.Index,
            });
        }

        private bool StartState() {
            var isTrue = false;
            for (int i = 0; i < wirebugList.Count; i++) {
                var cdTime = wirebugList[i];
                if (cdTime <= 0f) {
                    wirebugList[i] = WirebugCDTime;
                    isTrue = true;
                    break;
                }
            }
            TargetRpc(networkBase, new Proto_Wirebug.TargetRpc_UseWirebugState {
                isTrue = isTrue,
            });
            if (isTrue) {
                TargetRpc(networkBase, new Proto_Wirebug.TargetRpc_UseWirebug {
                    cdTimeList = wirebugList,
                });
            }
            return isTrue;
        }
    }
}