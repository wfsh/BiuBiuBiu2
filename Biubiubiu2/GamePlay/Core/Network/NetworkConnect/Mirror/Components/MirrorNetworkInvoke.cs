using System;
using kcp2k;
using Mirror.SimpleWeb;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.PerfAnalyzer;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class MirrorNetworkInvoke : ComponentBase {
        private int perf_ProtoCount = PerfAnalyzerKey.StringToHash("消息耗时");
        private int perf_ProtoTime = PerfAnalyzerKey.StringToHash("消息处理数量");
        public int frameInvokeCount = 0;
        public float frameInvokeTime = 0f;
        private float perfTime = 0.0f;

        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            CheckProtoPerf();
        }

        private void CheckProtoPerf() {
            frameInvokeTime += NetworkBase.FrameInvokeTime;
            frameInvokeCount = Math.Max(frameInvokeCount, NetworkBase.FrameInvokeCount);
            NetworkBase.FrameInvokeTime = 0f;
            NetworkBase.FrameInvokeCount = 0;
            if (perfTime > 0f) {
                perfTime -= Time.deltaTime;
                return;
            }
            perfTime = 0.3f;
            PerfAnalyzerAgent.SetCustomRecorder(perf_ProtoCount, (frameInvokeTime * 1000f));
            PerfAnalyzerAgent.SetCustomRecorder(perf_ProtoTime, frameInvokeCount);
            frameInvokeTime = 0f;
            frameInvokeCount = 0;
        }
    }
}