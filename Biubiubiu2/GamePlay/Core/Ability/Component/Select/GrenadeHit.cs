using System;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class GrenadeHit : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public bool IsHitBomb;
            public int FireGpoId;
            public int FireGpoTeamId;
            public Action CallBack;
        }
        private bool isHitBomb = false;
        public Action CallBack;
        public int fireGpoId;
        private int fireGpoTeamId;

        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            fireGpoTeamId = initData.FireGpoTeamId;
            SetData(initData.IsHitBomb, initData.FireGpoId, initData.CallBack);
        }

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
            this.CallBack = null;
        }

        public void SetData(bool isHitBome, int fireGpoId, Action callBack) {
            this.isHitBomb = isHitBome;
            this.CallBack = callBack;
            this.fireGpoId = fireGpoId;
        }

        private void OnHitCollision(Collision collision) {
            HitGameObj(collision.gameObject);
        }

        private void OnHitCollider(Collider collision) {
            HitGameObj(collision.gameObject);
        }

        protected virtual void HitGameObj(GameObject gameObj) {
            if (this.CallBack == null) {
                return;
            }
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null) {
                if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                    return;
                }
                if (hitType.MyEntity.GetGPOID() == this.fireGpoId) {
                    return;
                }
                if (fireGpoTeamId != 0 && fireGpoTeamId == hitType.MyEntity.GetTeamID()) {
                    return;
                }
            }
            if (this.isHitBomb) {
                this.CallBack?.Invoke();
            }
        }
    }
}