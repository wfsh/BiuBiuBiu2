using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ObjSizeForCamera : MonoBehaviour {
        private Transform _gameTran;
        private Transform mainCameraTran;
        private Camera mainCamera;

        [Tooltip("最小距离")]
        public float MinDistance = 2.0f;

        [Tooltip("最大距离")]
        public float MaxDistance = 30.0f;

        [Tooltip("最小尺寸")]
        public float MinScale = 0.2f;

        [Tooltip("最大尺寸")]
        public float MaxScale = 1.0f;

        [Tooltip("是否受倍镜影响")]
        public bool IsFieldOfView = true;

        [Tooltip("是否需要在update中实时刷新大小")]
        public bool IsNeedUpdateScale = false;

        public float DeployTime = -1.0f;
        public float MaxDeployTime = 1.0f;
        private float _deployTime = 0.0f;
        public bool IsBreak = false;
        private Vector3 defaultScale;
        public bool isBubble = true;
        public bool isShowUIPanel = false;

        private void OnEnable() {
            _gameTran = transform;
            GetMainCameraTran();
            Invoke("upObjScale", 0);
        }

        void OnDestroy() {
            _gameTran = null;
            mainCameraTran = null;
            mainCamera = null;
        }

        void Update() {
            if (IsNeedUpdateScale == false || IsBreak) {
                return;
            }
            if (DeployTime == -1) {
                return;
            }
            if (_deployTime > 0) {
                _deployTime -= Time.deltaTime;
            } else {
                _deployTime = Random.Range(DeployTime, MaxDeployTime);
                upObjScale();
            }
        }

        public void GetMainCameraTran() {
            if (mainCameraTran != null) {
                return;
            }
            mainCamera = Camera.main;
            if (mainCamera != null) {
                mainCameraTran = mainCamera.transform;
            }
        }

        void upObjScale() {
            if (mainCameraTran == null || _gameTran == null || IsBreak || isShowUIPanel) {
                return;
            }
            defaultScale = _gameTran.localScale;
            var point = _gameTran.position;
            var distance = Vector3.Distance(point, mainCameraTran.position);
            distance = Mathf.Clamp(distance, MinDistance, MaxDistance);
            var ratio = (distance - MinDistance) / (MaxDistance - MinDistance);
            var scale = MinScale + (MaxScale - MinScale) * ratio;
            var fovRatio = 1f;
            if (IsFieldOfView) {
                fovRatio = mainCamera.fieldOfView / BaseCameraRatio;
                fovRatio += (1 - fovRatio) * 0.2f;
            }
            if (!float.IsNaN(scale * fovRatio)) {
                _gameTran.localScale = Vector3.one * scale * fovRatio;
            }
        }

        private float BaseCameraRatio {
            get {
                var hFOVrad = 90f * Mathf.Deg2Rad;
                var camH = Mathf.Tan(hFOVrad * 0.5f) / ((float)Screen.width / (float)Screen.height);
                var vFOVrad = Mathf.Atan(camH) * 2f;
                var fov = vFOVrad * Mathf.Rad2Deg;
                if (!float.IsNaN(fov)) {
                    return fov;
                } else {
                    return 1;
                }
            }
        }
    }
}