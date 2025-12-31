using System;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class SelectForCollider : ComponentBase {
        private Action<GameObject> onHitTargetCallBack;
        private Action<IGPO> onHitGPOCallBack;

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            var entity = (EntityBase)iEntity;
            var collision = entity.gameObject.AddComponent<CollisionEnterCheck>();
            collision.Init(OnHitCollision, null, null);
            var collider = entity.gameObject.AddComponent<ColliderEnterCheck>();
            collider.Init(OnHitCollider, null, null);
        }

        protected override void OnClear() {
            base.OnClear();
            onHitTargetCallBack = null;
            onHitGPOCallBack = null;
        }

        public void HitTargetCallBack(Action<GameObject> hitTargetCallBack) {
            onHitTargetCallBack = hitTargetCallBack;
        }

        public void HitGPOCallBack(Action<IGPO> hitGPOCallBack) {
            onHitGPOCallBack = hitGPOCallBack;
        }

        private void OnHitCollision(Collision collision) {
            if (onHitGPOCallBack != null) {
                var hitType = collision.gameObject.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        return;
                    }
                    if (hitType.MyEntity != null && hitType.MyEntity.GetGPO() != null) {
                        onHitGPOCallBack.Invoke(hitType.MyEntity.GetGPO());
                    }
                }
            }
            onHitTargetCallBack?.Invoke(collision.gameObject);
        }

        private void OnHitCollider(Collider collision) {
            if (onHitGPOCallBack != null) {
                var hitType = collision.gameObject.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        return;
                    }
                    if (hitType.MyEntity != null && hitType.MyEntity.GetGPO() != null) {
                        onHitGPOCallBack.Invoke(hitType.MyEntity.GetGPO());
                    }
                }
            }
            onHitTargetCallBack?.Invoke(collision.gameObject);
        }
    }
}