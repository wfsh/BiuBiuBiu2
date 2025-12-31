using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSceneGetScenePoints : ServerNetworkComponentBase {
        private List<ScenePoint> points = new List<ScenePoint>();
        private Dictionary<int, List<ScenePoint>> wavePoints = new Dictionary<int, List<ScenePoint>>();
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Scene.GetRangeScenePoint>(OnScenePointsCallBack);
            mySystem.Register<SE_Scene.SendScenePoints>(OnSendScenePoints);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Scene.GetRangeScenePoint>(OnScenePointsCallBack);
            mySystem.Unregister<SE_Scene.SendScenePoints>(OnSendScenePoints);
        }
        
        private void OnSendScenePoints(ISystemMsg body, SE_Scene.SendScenePoints ent) {
            points = ent.ScenePoints;
            RenderPointType();
        }

        private void RenderPointType() {
            foreach (var point in points) {
                var pointTypes = point.PointTypes;
                if (string.IsNullOrEmpty(pointTypes)) {
                    pointTypes = "0";
                }
                var types = pointTypes.Split(",");
                foreach (var type in types) {
                    var intValue = 0;
                    int.TryParse(type, out intValue);
                    List<ScenePoint> list;
                    if (wavePoints.TryGetValue(intValue, out list) == false) {
                        list = new List<ScenePoint>();
                        wavePoints[intValue] = list;
                    }
                    list.Add(point);
                }
            }
            var count = 0;
            foreach (var data in wavePoints) {
                count += data.Value.Count;
                Debug.LogError("点位类型：" + data.Key + " 点位数量：" + data.Value.Count);
            }
        }
        
        private void OnScenePointsCallBack(SM_Scene.GetRangeScenePoint ent) {
            var ret = new List<ScenePoint>();
            var points = GetPointsByType(ent.PointTypes);
            foreach (var pt in points) {
                var distance = Vector3.Distance(pt.Point, ent.Point);
                if (ent.MaxRange > 0 && distance > ent.MaxRange) {
                    continue;
                }
                if (ent.MinRange > 0 && distance < ent.MinRange) {
                    continue;
                }
                ret.Add(pt);
            }
            ent.CallBack?.Invoke(ret);
        }
        
        private List<ScenePoint> GetPointsByType(List<int> checkTypes) {
            if (checkTypes == null || checkTypes.Count == 0) {
                checkTypes = new List<int> {0};
            }
            var list = new List<ScenePoint>();
            for (int i = 0; i < checkTypes.Count; i++) {
                var checkType = checkTypes[i];
                List<ScenePoint> checkList = null;
                if (wavePoints.TryGetValue(checkType, out checkList)) {
                    list.AddRange(checkList);
                }
            }
            return list;
        }
    }
}