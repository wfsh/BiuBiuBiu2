using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientBloodSplatter : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public int BloodValue;
            public int HitGpoId;
            public int HitItemId;
            public Vector3 DiffPos;
        }

        private InitData initData;
        private C_Ability_Base abSystem;
        protected override void OnAwake() {
            base.OnAwake();
            initData = (InitData)initDataBase;
            abSystem = (C_Ability_Base)mySystem;
        }

        protected override void OnStart() {
            base.OnStart();
            
            var hitPoint = Vector3.zero;
            MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                GpoId = initData.HitGpoId,
                CallBack = gpo => {
                    if (gpo != null && !gpo.IsClear()) {
                        hitPoint = gpo.GetPoint();
                    }
                }
            });
            
            if (hitPoint == Vector3.zero) {
                return;
            }
            
            hitPoint += initData.DiffPos;
            iEntity.SetPoint(hitPoint);
            
            IGPO attackGpo = null;
            MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                GpoId = abSystem.FireGpoId,
                CallBack = gpo => attackGpo = gpo
            });
            
            MsgRegister.Dispatcher(new CM_GPO.SendBloodSplatterData() {
                BloodValue = initData.BloodValue,
                FireGpoId = abSystem.FireGpoId,
                HitGpoId = initData.HitGpoId,
                HitPoint = hitPoint,
                HitItemId = initData.HitItemId,
            });
        }
    }
}