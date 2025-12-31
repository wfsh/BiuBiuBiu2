using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class CharacterNetwork : NetworkBase, INetworkCharacter {
        private NetworkTransformBase mNetworkTransform;
        private bool isCharacterReady = false;
        override protected void OnStart() {
            MsgRegister.Dispatcher(new M_Character.SetNetwork {
                iNetwork = this,
            });
            mNetworkTransform = GetComponent<NetworkTransformBase>();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Dispatcher(new M_Character.DestoryNetwork {
                NetworkId = GetNetworkId(),
            });
            mNetworkTransform = null;
        }

        public void SetPoint(Vector3 point) {
            if (isDestroy) {
                return;
            }
            transform.position = point;
        }

        public void SetRota(Quaternion rota) {
            if (isDestroy) {
                return;
            }
            transform.rotation = rota;
        }

        public Vector3 GetPoint() {
            return transform.position;
        }

        public Quaternion GetRota() {
            return transform.rotation;
        }

        public bool IsLocalPlayer() {
            return isLocalPlayer;
        }

        public void Cmd(ICmd proto) {
            if (isDestroy || NetworkClient.active == false) {
                return;
            }
            MsgRegister.Dispatcher(new M_Network.CMDSerialize {
                ProtoDoc = proto, Channel = proto.GetChannel(), CallBack = OnCMDSerializeCallBack,
            });
        }

        private void OnCMDSerializeCallBack(int channel, byte[] bytes) {
            if (channel == NetworkData.Channels.Unreliable) {
                CmdBytesUnreliable(bytes);
            } else {
                CmdBytes(bytes);
            }
        }

        [Command]
        private void CmdBytes(byte[] bytes) {
            if (isDestroy) {
                return;
            }
            MsgRegister.Dispatcher(new M_Network.CMDUnSerialize {
                Datas = bytes, CallBack = OnCMDUnSerializeCallBack,
            });
        }

        [Command(channel = Channels.Unreliable)]
        private void CmdBytesUnreliable(byte[] bytes) {
            if (isDestroy) {
                return;
            }
            MsgRegister.Dispatcher(new M_Network.CMDUnSerialize {
                Datas = bytes, CallBack = OnCMDUnSerializeCallBack,
            });
        }

        private void OnCMDUnSerializeCallBack(IProto_Doc proto) {
            try {
                PerfAnalyzerAgent.BeginSample(proto.GetID());
                SendProtoCallBack(proto);
            } catch (Exception e) {
                Debug.LogError($"CMD 协议执行出错{proto.GetID()} _E:{e}");
            } finally {
                PerfAnalyzerAgent.EndSample(proto.GetID());
            }
        }

        public void SetInterpolatePositionState(bool state) {
            if (isDestroy || mNetworkTransform == null) {
                return;
            }
            mNetworkTransform.interpolatePosition = state;
        }
        
        public void SetCharacterReady(bool ready) {
            isCharacterReady = ready;
        }

        public bool IsCharacterReady() {
            return isCharacterReady;
        }
    }
}