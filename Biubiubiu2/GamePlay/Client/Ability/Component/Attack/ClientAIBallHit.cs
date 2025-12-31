using System;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIBallHit : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public uint MonsterPid;
            public Action CallBack;
        }
        private bool isHit = false;
        private uint MonsterPid = 0;
        private Action endCallBack;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.MonsterPid, initData.CallBack);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entity = (EntityBase)iEntity;
            entity.gameObject.layer = LayerMask.NameToLayer("ClientLayer");
            var collision = entity.gameObject.AddComponent<CollisionEnterCheck>();
            collision.Init(OnHitCollision, null, null);
            var collider = entity.gameObject.AddComponent<ColliderEnterCheck>();
            collider.Init(OnHitCollider, null, null);
        }

        protected override void OnClear() {
            base.OnClear();
            endCallBack = null;
        }

        public void SetData(uint monsterPid, Action callBack) {
            MonsterPid = monsterPid;
            endCallBack = callBack;
        }

        private void OnHitCollision(Collision collision) {
            HitGameObj(collision.gameObject);
        }

        private void OnHitCollider(Collider collision) {
            HitGameObj(collision.gameObject);
        }

        private void HitGameObj(GameObject gameObj) {
            if (isHit) {
                return;
            }
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null) {
                if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                    return;
                }
                if (hitType.MyEntity != null && hitType.MyEntity.GetGPO() != null) {
                    OnHitGPOCallBack(hitType.MyEntity.GetGPO());
                }
            }
            OnHitTargetCallBack(gameObj);
            isHit = true;
        }

        private void OnHitTargetCallBack(GameObject ray) {
            TakeOutMonster();
        }

        private void OnHitGPOCallBack(IGPO gpo) {
            if (MonsterPid != 0) {
                TakeOutMonster();
            } else {
                if (gpo.GetGPOType() == GPOData.GPOType.AI) {
                    endCallBack();
                }
            }
        }

        private void TakeOutMonster() {
            if (MonsterPid == 0) {
                return;
            }
            endCallBack?.Invoke();
        }
    }
}