using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class WirebugLine : MonoBehaviour {
        private const float speed = 50f;
        public int ID { get; private set; }

        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private Transform targetTransform;

        public Vector3 TargetPoint { get; private set; }
        private Transform myTransform;
        private bool isEnabled = true;
        private Vector3[] movePoints = new Vector3[2];
        private bool isTargetMoveEndPoint = false;
        public Action<Vector3> OnTargetObjMoveEnd;

        public void Init() {
            isEnabled = false;
            myTransform = transform;
            targetTransform.localPosition = Vector3.zero;
            if (lineRenderer == null) {
                Debug.LogError("LineRenderer is not assigned.");
            }
            if (targetTransform == null) {
                Debug.LogError("Target Transform is not assigned.");
            }
        }

        public void SetID(int id) {
            ID = id;
        }

        public void Clear() {
            myTransform = null;
        }

        public void Update() {
            if (isEnabled == false) {
                return;
            }
            UpdateLineRenderer();
            MoveTargetTowardsPoint();
        }

        private void MoveTargetTowardsPoint() {
            if (!isTargetMoveEndPoint) {
                var movePoint = Vector3.MoveTowards(targetTransform.position, TargetPoint, speed * Time.deltaTime);
                if (Vector3.Distance(movePoint, TargetPoint) < 0.1f) {
                    targetTransform.position = TargetPoint;
                    isTargetMoveEndPoint = true;
                    OnTargetObjMoveEnd?.Invoke(TargetPoint);
                } else {
                    targetTransform.position = movePoint;
                }
            } else {
                targetTransform.position = TargetPoint;
            }
        }

        public void MoveTo(Vector3 target) {
            isEnabled = true;
            if (lineRenderer.enabled == false) {
                lineRenderer.enabled = true;
            }
            this.isTargetMoveEndPoint = false;
            this.TargetPoint = target;
        }

        public void SetPoint(Vector3 point) {
            myTransform.position = point;
        }

        public float LineDistance() {
            var checkPoint = myTransform.position;
            checkPoint.y = TargetPoint.y;
            return Vector3.Distance(checkPoint, TargetPoint);
        }

        private void UpdateLineRenderer() {
            movePoints[0] = myTransform.position;
            movePoints[1] = targetTransform.position;
            lineRenderer.positionCount = movePoints.Length;
            lineRenderer.SetPositions(movePoints); // 设置抛物线上的点到 LineRenderer
        }
    }
}