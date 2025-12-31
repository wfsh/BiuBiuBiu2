using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class LineRendererParabola : MonoBehaviour {
        [SerializeField]
        private float addHeight = 1.0f;
        [SerializeField]
        private LineRenderer lineRenderer;
        private Vector3[] movePoints;
        private int positionCount = 0;
        private Vector3 startPoint = Vector3.zero;
        private Vector3 targetPoint = Vector3.zero;
        private Transform myTransform;
        private bool isEnabled = true;

        public void Init() {
            myTransform = transform;
            movePoints = new Vector3[30];
        }

        public void Clear() {
            movePoints = null;
            myTransform = null;
        }

        public void Update() {
            if (isEnabled == false) {
                return;
            }
            UpdateLineRendererForEndPoint();
        }

        public void Enabled(bool isTrue) {
            isEnabled = isTrue;
            if (isEnabled == false) {
                lineRenderer.positionCount = 0;
                lineRenderer.enabled = false;
            }
        }

        public Vector3[] GetMovePoints() {
            var points = new Vector3[positionCount];
            Array.Copy(movePoints, 0, points, 0, positionCount);
            return points;
        }

        public void SetStartPoint(Vector3 point, Vector3 endPoint) {
            if (isEnabled && lineRenderer.enabled == false) {
                lineRenderer.enabled = true;
            }
            myTransform.position = point;
            startPoint = point;
            targetPoint = endPoint;
#if UNITY_EDITOR
            Debug.DrawLine(point, endPoint, Color.green);
#endif
        }

        private void UpdateLineRendererForEndPoint() {
            var distance = Vector3.Distance(startPoint, targetPoint);
            var pointCount = 1;
            if (distance <= 10) {
                pointCount = 2 + Mathf.CeilToInt(distance);
            } else {
                pointCount = 12 + Mathf.CeilToInt(distance * 0.15f);
                if (pointCount >= movePoints.Length) {
                    pointCount = movePoints.Length - 1;
                }
            }
            var height = distance * addHeight;
            var nowPoint = startPoint;
            for (int i = 0; i <= pointCount; i++) {
                var t = (float)i / (float)pointCount;
                var checkPoint = CalculatePoint(startPoint, targetPoint, height, t);
                movePoints[i] = checkPoint;
#if UNITY_EDITOR
                Debug.DrawLine(nowPoint, checkPoint, i % 2 == 0 ? Color.blue : Color.green);
                nowPoint = checkPoint;
#endif
            }
            positionCount = pointCount;
            lineRenderer.positionCount = pointCount;
            lineRenderer.SetPositions(movePoints); // 设置抛物线上的点到 LineRenderer
        }

        private Vector3 CalculatePoint(Vector3 start, Vector3 end, float height, float t) {
            var x = Mathf.Lerp(start.x, end.x, t);
            var y = Mathf.Lerp(start.y, end.y, t);
            var z = Mathf.Lerp(start.z, end.z, t);
            var curveHeight = height * (t - t * t); // 根据 t 调整高度
            return new Vector3(x, y + curveHeight, z);
        }
    }
}