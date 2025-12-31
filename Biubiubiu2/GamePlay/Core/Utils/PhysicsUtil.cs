using System;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public static class PhysicsUtil {
        private static readonly RaycastHit[] tmpHits = new RaycastHit[10];

        public static bool CheckBlocked(Vector3 startPoint, Vector3 entPoint, Func<RaycastHit, bool> ignoreFunc, int layerMask, out Vector3 hitPoint) {
            hitPoint = Vector3.zero;
            var direction = entPoint - startPoint;
            var distance  = direction.magnitude;
            var hitCount = Physics.RaycastNonAlloc(startPoint, direction, tmpHits, distance, layerMask);
            if (hitCount > 0 && CheckHitObject(hitCount, distance, ignoreFunc, out hitPoint)) {
                return true;
            }
            return false;
        }

        private static bool CheckHitObject(int hitCount, float maxDistance, Func<RaycastHit, bool> ignoreFunc, out Vector3 hitPoint) {
            var minDistance = maxDistance;
            var isHit = false;
            hitPoint = Vector3.zero;
            var hitRay = new RaycastHit();
            for (int i = 0; i < hitCount; i++) {
                var ray = tmpHits[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                if (ignoreFunc.Invoke(ray)) {
                    continue;
                }
                if (!isHit || minDistance > ray.distance) {
                    minDistance = ray.distance;
                    hitPoint = ray.point;
                    isHit = true;
                }
            }
            return isHit;
        }
    }
}