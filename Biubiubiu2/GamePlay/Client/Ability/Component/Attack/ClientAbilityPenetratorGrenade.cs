using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class ClientAbilityPenetratorGrenade : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public float Speed;
        }
        private float speed;
        private Vector3 forward = Vector3.zero;
        private Vector3 startPoint = Vector3.zero;
        private bool isHit = false;
        public Vector3 hitPoint;
        public Quaternion hitRota;
        public int hitGpoID;
        public int hitGpoType;
        public EntityBase entity;
        public C_Ability_Base abilitySystem;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetSpeed(initData.Speed);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            abilitySystem = (C_Ability_Base)mySystem;
            startPoint = iEntity.GetPoint();
            forward = iEntity.GetRota() * Vector3.forward;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            entity = (EntityBase)iEntity;
            GetHitGPO();
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Ability.Rpc_PenetratorGrenadeHit.ID, OnPenetratorGrenadeHitCallBack);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            abilitySystem = null;
            RemoveProtoCallBack(Proto_Ability.Rpc_PenetratorGrenadeHit.ID, OnPenetratorGrenadeHitCallBack);
        }

        public void SetSpeed(float speed) {
            this.speed = speed;
        }

        private void OnUpdate(float delta) {
            if (isHit) {
            } else {
                Move(delta);
            }
        }

        private void Move(float delta) {
            var point = iEntity.GetPoint();
            point += forward * this.speed * delta;
            iEntity.SetPoint(point);
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, point, Color.red);
#endif
        }

        private void OnPenetratorGrenadeHitCallBack(INetwork network, IProto_Doc protoDoc) {
            var data = (Proto_Ability.Rpc_PenetratorGrenadeHit)protoDoc;
            if (abilitySystem.AbilityId != data.abilityId) {
                return;
            }
            isHit = true;
            hitPoint = data.hitPoint;
            hitRota = data.hitRota;
            hitGpoID = data.hitGpoID;
            hitGpoType = data.hitGpoType;
            GetHitGPO();
        }

        private void GetHitGPO() {
            if (entity == null || isHit == false) {
                return;
            }
            if (hitGpoID != 0) {
                MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                    GpoId = hitGpoID,
                    CallBack = GetGPOCallBack,
                });
            } else {
                iEntity.SetPoint(hitPoint);
                iEntity.SetRota(hitRota);
            }
        }

        private void GetGPOCallBack(IGPO gpo) {
            if (gpo == null) {
                return;
            }
            var tran = gpo.GetBodyTran((GPOData.PartEnum)hitGpoType);
            if (tran == null) {
                var clientGPO = (ClientGPO)gpo;
                Debug.LogError($"{clientGPO.GetName()} 没有该部件 {hitGpoType}");
                return;
            }
            entity.SetParent(tran);
            entity.SetLocalPoint(hitPoint);
            entity.SetLocalRota(hitRota);
            entity.SetLocalScale(Vector3.one);
        }
    }
}