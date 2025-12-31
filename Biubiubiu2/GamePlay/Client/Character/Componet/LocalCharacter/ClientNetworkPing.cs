using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientNetworkPing : ClientCharacterComponent {
        private Stopwatch stopwatch = null;
        private float PingFrequency = 1.0f; // 最高延迟 1 秒
        private float lastPingTime = 0f;
        private List<double> pings = new List<double>();
        private List<SendData> sendPings = new List<SendData>();
        private ushort ping = 0;
        private bool isCmdIng = false;
        private byte sendIndex = 0;
        
        private struct SendData {
            public double time;
            public byte index;
        }

        protected override void OnStart() {
            base.OnStart();
            stopwatch = new Stopwatch();
            stopwatch.Start();
            OnClientPong(0f);
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            sendPings.Clear();
            stopwatch.Stop();
            stopwatch = null;
            RemoveUpdate(OnUpdate);
            this.RemoveProtoCallBack(Proto_Character.TargetRpc_Ping.ID, TargetRpcPingCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Character.TargetRpc_Ping.ID, TargetRpcPingCallBack);
        }

        private void TargetRpcPingCallBack(INetwork network, IProto_Doc data) {
            var rpcData = (Proto_Character.TargetRpc_Ping)data;
            var index = rpcData.index;
            var lostIndex = 0;
            double sendTime = 0.0f;
            var isCheck = false;
            while (sendPings.Count > 0) {
                var sendData = sendPings[0];
                sendPings.RemoveAt(0);
                if (index == sendData.index) {
                    sendTime = sendData.time;
                    isCheck = true;
                    break;
                }
                lostIndex++;
            }
            if (isCheck == false) {
                return;
            }
            var now = LocalTime();
            var nowRtt = now - sendTime;
            OnClientPong(nowRtt);
        }

        private void OnUpdate(float deltaTime) {
            if (characterNetwork == null || characterNetwork.IsDestroy()) {
                return;
            }
            var t = Time.time - lastPingTime;
            if (Time.time - lastPingTime >= PingFrequency) {
                // 1 秒发送一次
                if (isCmdIng) {
                    OnClientPong(PingFrequency);
                }
                SendPing();
            }
        }

        private void SendPing() {
            isCmdIng = true;
            lastPingTime = Time.time;
            if (Application.internetReachability == NetworkReachability.NotReachable) {
                OnClientPong(PingFrequency);
            } else {
                sendIndex++;
                if (sendIndex > 255) {
                    sendIndex = 0;
                }
                sendPings.Add(new SendData {
                    time = LocalTime(),
                    index = sendIndex,
                });
                Cmd(new Proto_Character.Cmd_Ping {
                    index = sendIndex,
                });
            }
        }

        private void OnClientPong(double nowRtt) {
            isCmdIng = false;
            if (nowRtt > PingFrequency) {
                nowRtt = PingFrequency;
            }
            pings.Add(nowRtt);
            if (pings.Count > 5) {
                pings.RemoveAt(0);
            }
            ping = GetPing();
            PerfAnalyzerAgent.SetPing(ping);
            MsgRegister.Dispatcher(new CM_Network.SendPing {
                Ping = ping
            });
        }

        // 5 次 rtt 取平均
        private ushort GetPing() {
            double value = 0f;
            var len = pings.Count;
            for (int i = 0; i < len; i++) {
                value += pings[i];
            }
            var avgPing = (value / len);
            return (ushort)(avgPing * 1000);
        }

        private double LocalTime() {
            return stopwatch.Elapsed.TotalSeconds;
        }
    }
}