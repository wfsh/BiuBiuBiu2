using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public static class CurveUtil {
        private static readonly List<Vector3> tmpPoints = new List<Vector3>();

        public static Vector3[] GetPointsForEndPoint(Vector3 startPoint, Vector3 endPoint, int count, float addHeight, int layerMask) {
            var distance = Vector3.Distance(startPoint, endPoint);
            int pointCount = CalculatePointCount(count, distance);
            var height = Mathf.Max(distance * addHeight / 2, addHeight);
            tmpPoints.Clear();
            tmpPoints.Add(startPoint);
            var prevPoint = startPoint;
            for (int i = 1; i < pointCount; i++) {
                var t = i / (pointCount - 1f);
                var checkPoint = CalculatePoint(startPoint, endPoint, height, t * t);
                if (PhysicsUtil.CheckBlocked(prevPoint, checkPoint, IgnoreFunc, layerMask, out var hitPoint)) {
                    tmpPoints.Add(hitPoint);
                    break;
                }
                prevPoint = checkPoint;
                tmpPoints.Add(checkPoint);
            }
            return tmpPoints.ToArray();
        }

        private static int CalculatePointCount(int count, float distance) {
            int pointCount;
            if (distance <= 10) {
                pointCount = 2 + Mathf.CeilToInt(distance);
            } else {
                pointCount = 12 + Mathf.CeilToInt(distance * 0.15f);
                if (pointCount >= count) {
                    pointCount = count - 1;
                }
            }
            return pointCount;
        }

        private static Vector3 CalculatePoint(Vector3 start, Vector3 end, float height, float t) {
            var x = Mathf.Lerp(start.x, end.x, t);
            var y = Mathf.Lerp(start.y, end.y, t);
            var z = Mathf.Lerp(start.z, end.z, t);
            var curveHeight = height * (t - t * t); // 根据 t 调整高度
            return new Vector3(x, y + curveHeight, z);
        }

        private static bool IgnoreFunc(RaycastHit hit) {
            var hitType = hit.collider.gameObject.GetComponent<HitType>();
            if (hitType != null) {
                // 忽略空气墙与角色
                if (hitType.Layer == GPOData.LayerEnum.Ignore || hitType.MyEntity != null) {
                    return true;
                }
            }
            return false;
        }
    }
}