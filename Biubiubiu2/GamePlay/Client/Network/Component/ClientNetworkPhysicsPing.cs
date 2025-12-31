using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientNetworkPhysicsPing : ComponentBase {
        private const short delayPingTime = 1000;
        private float delayPingTimeValue = 0f;
        private List<short> pings = new List<short>();
        private ushort pingTime = 0;
        private float measurePingTime = 0f;
        private bool measurePingDone = false;
        private int messageIndex = 0;
        private string address = "";
        
        protected override void OnStart() {
            base.OnStart();
            InitAddress();
        }
        
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
        }
        
        private void InitAddress() {
            if (string.IsNullOrEmpty(NetworkData.Config.IP)) {
                return;
            }
            address = $"https://{NetworkData.Config.IP}";
            PerfAnalyzerAgent.SetPhysicsPing(0);
            PerfAnalyzerAgent.SetLog("物理延迟开始:" + address);
            AddUpdate(OnUpdate);
        }
        
        private void OnUpdate(float delta) {
            if (address == "") {
                return;
            }
            if (measurePingDone) {
                SetPing((short)measurePingTime);
            }
            if ((Time.time - delayPingTimeValue) * 1000 > delayPingTime) {
                delayPingTimeValue = Time.time;
                if (measurePingDone == false) {
                    SetPing(delayPingTime);
                }
                SendPing();
            }
        }
        
        private void SetPing(short ping) {
            if (ping < 0f) {
                ping = delayPingTime;
            }
            pings.Add(ping);
            if (pings.Count > 5) {
                pings.RemoveAt(0);
            }
            pingTime = GetPing();
            PerfAnalyzerAgent.SetPhysicsPing(pingTime);
            MsgRegister.Dispatcher(new CM_Network.SendPhysicsPing() {
                Ping = pingTime
            });
        }
        
        private ushort GetPing() {
            var value = 0;
            var len = pings.Count;
            for (int i = 0; i < len; i++) {
                value += pings[i];
            }
            var avgPing = value / len;
            return (ushort)avgPing;
        }
        
        private void SendPing() {
            measurePingDone = false;
            messageIndex++;
            //AssetManager.SendMeasureHttpPing(address, messageIndex, (index, ping) => {
            //    if (index != messageIndex) {
            //        return;
            //    }
            //    measurePingDone = true;
            //    measurePingTime = ping;
            //});
        }
    }
}