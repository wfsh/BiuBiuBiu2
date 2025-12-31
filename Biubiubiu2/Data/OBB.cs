using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    [Serializable]
    public struct OBB {
        [SerializeField]
        private Vector3 extents;
        [SerializeField]
        private Matrix4x4 worldToLocal; // 包围盒的逆变换矩阵

        // 检测点是否在 OBB 内
        public bool Contains(Vector3 point) {
            // 将点从世界坐标转换到 OBB 的局部坐标系
            Vector3 pointToLocal = worldToLocal.MultiplyPoint3x4(point);
            // 在局部坐标系中做 AABB 检测
            return Mathf.Abs(pointToLocal.x) <= extents.x &&
                   Mathf.Abs(pointToLocal.y) <= extents.y &&
                   Mathf.Abs(pointToLocal.z) <= extents.z;
        }

        public static OBB FromGameObject(GameObject go) {
            Transform transform = go.transform;
            return new OBB {
                extents = transform.localScale * 0.5f,
                worldToLocal = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse
            };
        }

        public static void ToGameObject(GameObject go, OBB obb) {
            Transform transform = go.transform;
            var localToWorld = obb.worldToLocal.inverse;
            transform.rotation = localToWorld.rotation;
            transform.position = localToWorld.GetPosition();
            transform.localScale = obb.extents * 2f;
        }
    }
}