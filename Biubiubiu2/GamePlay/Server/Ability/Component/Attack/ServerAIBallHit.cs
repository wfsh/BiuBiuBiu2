using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIBallHit : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public uint MonsterPid;
            public Action EndCallBack;
        }
        private bool isHit = false;
        private uint MonsterPid = 0;
        private Action endCallBack;
        private S_Ability_Base myAbility;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.MonsterPid, initData.EndCallBack);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            myAbility = (S_Ability_Base)mySystem;
            var entity = (EntityBase)iEntity;
            entity.gameObject.layer = LayerMask.NameToLayer("ServerLayer");
            var collision = entity.gameObject.AddComponent<CollisionEnterCheck>();
            collision.Init(OnHitCollision, null, null);
            var collider = entity.gameObject.AddComponent<ColliderEnterCheck>();
            collider.Init(OnHitCollider, null, null);
        }

        protected override void OnClear() {
            base.OnClear();
            myAbility = null;
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
            if (isHit || isClear) {
                return;
            }
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null) {
                if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                    return;
                }
                if (hitType.MyEntity != null && hitType.MyEntity.GetGPO() != null) {
                    OnHitGPOCallBack((ServerGPO)hitType.MyEntity.GetGPO());
                    isHit = true;
                    return;
                }
            }
            OnHitTargetCallBack(gameObj);
            isHit = true;
        }

        private void OnHitTargetCallBack(GameObject ray) {
            TakeOutMonster();
        }

        private void OnHitGPOCallBack(ServerGPO gpo) {
            if (MonsterPid != 0) {
                TakeOutMonster();
            } else {
                if (gpo.GetGPOType() == GPOData.GPOType.AI) {
                    gpo.Dispatcher(new SE_AI.Event_CatchAI {
                        CatchGPO = myAbility.FireGPO
                    });
                    endCallBack();
                }
            }
        }

        private void TakeOutMonster() {
            if (MonsterPid == 0) {
                return;
            }
            endCallBack();
        }
    }
}