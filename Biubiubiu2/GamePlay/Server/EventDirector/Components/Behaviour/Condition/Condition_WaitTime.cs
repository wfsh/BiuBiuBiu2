using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Condition_WaitTime : EventDirectorCondition {
        public struct InitData : IEventDirectorData {
            public float WaitTime;
            public void Serialize (string value) {
                WaitTime = float.Parse(value);
            }
        }
        private float waitTime = 0f;
        private InitData useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = SerializeData<InitData>();
        }

        public override bool CheckCondition() {
            if (waitTime <= 0f) {
                waitTime = Time.realtimeSinceStartup + useMData.WaitTime;
            }
            if (compareType == EventDirectorData.CompareType.Less) {
                return Time.realtimeSinceStartup < waitTime;
            }
            return Time.realtimeSinceStartup >= waitTime;
        }
    }
}
