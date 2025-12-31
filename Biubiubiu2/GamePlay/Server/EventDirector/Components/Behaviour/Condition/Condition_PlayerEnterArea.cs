using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Condition_PlayerEnterArea : EventDirectorCondition {
        public struct InitData : IEventDirectorData {
            public Vector3 Point;
            public float Radius;
            public int Count; // 进入次数;
            public void Serialize (string value) {
                var arr = value.Split('&');
                var pointStr = arr[0];
                var pointArr = pointStr.Split(';');
                Point = new Vector3(float.Parse(pointArr[0]), float.Parse(pointArr[1]), float.Parse(pointArr[2]));
                Radius = float.Parse(arr[1]);
                Count = int.Parse(arr[2]);
            }
        }
        private int enterCount = 0;
        private InitData useMData;
        private Dictionary<int, bool> enterGpoIds = new Dictionary<int, bool>();

        protected override void OnAwake() {
            base.OnAwake();
            useMData = SerializeData<InitData>();
        }

        protected override void OnClear() {
            base.OnClear();
            enterGpoIds.Clear();
        }

        public override bool CheckCondition() {
            IGPO triggerGpo = null;
            enterCount = 0;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.IsClear() || gpo.GetGPOType() != GPOData.GPOType.Role) {
                    continue;
                }
                var distance = Vector3.Distance(gpo.GetPoint(), useMData.Point);
                if (distance <= useMData.Radius) {
                    enterCount++;
                    SetTriggerGPO(gpo);
                }
            }
            if (compareType == EventDirectorData.CompareType.Less) {
                return enterCount < useMData.Count;
            }
            return enterCount >= useMData.Count;
        }
    }
}
