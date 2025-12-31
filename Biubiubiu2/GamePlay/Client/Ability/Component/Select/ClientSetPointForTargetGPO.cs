using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientSetPointForTargetGPO : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public int TargetGPOId;
        }
        private IGPO targetGPO;
        private int targetGPOId;
        private float deltaTime = 0f;

        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetTargetGPOID(initData.TargetGPOId);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            targetGPO = null;
        }

        private void OnUpdate(float delayTime) {
            if (isSetEntityObj == false || targetGPOId <= 0) {
                return;
            }
            SelectLockGPO();
            if (targetGPO != null && targetGPO.IsClear() == false) {
                iEntity.SetPoint(targetGPO.GetPoint());
                iEntity.SetRota(targetGPO.GetRota());
            }
        }

        public void SetTargetGPOID(int id) {
            targetGPOId = id;
        }

        private void SelectLockGPO() {
            if (deltaTime > 0f) {
                return;
            }
            deltaTime = 0.5f;
            if (targetGPO != null) {
                return;
            }
            MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                GpoId = targetGPOId,
                CallBack = gpo => {
                    targetGPO = gpo;
                }
            });
        }
    }
}