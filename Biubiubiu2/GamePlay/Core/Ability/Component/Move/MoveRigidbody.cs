using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    // 直线匀速移动
    public class MoveRigidbody : ComponentBase {
        
        protected override void OnClear() {
        }

        public void SetData(float force) {
            EnableRCollider();
            var entity = (EntityBase)iEntity;
            var myRigidbody = entity.GetComponent<Rigidbody>();
            myRigidbody.useGravity = true;
            myRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            myRigidbody.AddForce(iEntity.GetForward() * force, ForceMode.Impulse);
        }

        private void EnableRCollider() {
            var entity = (EntityBase)iEntity;
            var collider = entity.GetComponent<Collider>();
            collider.enabled = true;
            collider.isTrigger = false;
        }
    }
}
