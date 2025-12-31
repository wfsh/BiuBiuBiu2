using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Action_SpawnSummerJokerStone : EventDirectorAction {
        public struct InitData : IEventDirectorData {
            public int SpawnGpo1Id;
            public int SpawnGpo2Id;
            public Vector3 Point;
            public float HeightAboveGround;
            public int LinkGpoMTypeId;
            public int TriggerCount;
            public int SpawnWaitTime;
            public void Serialize (string value) {
                var arr = value.Split('&');
                SpawnGpo1Id = int.Parse(arr[0]);
                SpawnGpo2Id = int.Parse(arr[1]);
                var pointStr = arr[2];
                var pointArr = pointStr.Split(';');
                Point = new Vector3(float.Parse(pointArr[0]), float.Parse(pointArr[1]), float.Parse(pointArr[2]));
                HeightAboveGround = float.Parse(arr[3]);
                LinkGpoMTypeId = int.Parse(arr[4]);
                TriggerCount = int.Parse(arr[5]);
                SpawnWaitTime = int.Parse(arr[6]);
            }
        }
        private InitData useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = SerializeData<InitData>();
        }
    }
}
