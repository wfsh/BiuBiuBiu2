using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIDrive : ServerNetworkComponentBase {
        private IGPO masterGPO;
        private bool isDriver;

        protected override void OnAwake() {
            Register<SE_AI.Event_TakeBack>(OnTakeBackCallBack);
            Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            Register<SE_AI.Event_DriverPointRota>(OnDriverPointRotaCallBack);
            Register<SE_AI.Event_GetDriveState>(OnGetDriveStateCallBack);
        }
        
        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var monsterSystem = (S_AI_Base)mySystem;
            masterGPO = monsterSystem.MasterGPO;
            if (masterGPO != null) {
                SetDriverGPO(true);
            }
        }

        protected override void OnClear() {
            if (isDriver) {
                SetDriverGPO(false);
            }
            Unregister<SE_AI.Event_DriverPointRota>(OnDriverPointRotaCallBack);
            Unregister<SE_AI.Event_TakeBack>(OnTakeBackCallBack);
            Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            Unregister<SE_AI.Event_GetDriveState>(OnGetDriveStateCallBack);
            masterGPO = null;
            base.OnClear();
        }

        private void OnDriverPointRotaCallBack(ISystemMsg body, SE_AI.Event_DriverPointRota ent) {
            if (masterGPO == null) {
                return;
            }
            iEntity.SetPoint(ent.Point);
            iEntity.SetRota(ent.Rota);
        }

        private void OnTakeBackCallBack(ISystemMsg body, SE_AI.Event_TakeBack ent) {
            SetDriverGPO(false);
        }

        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                SetDriverGPO(false);
            }
        }
        
        private void OnGetDriveStateCallBack(ISystemMsg body, SE_AI.Event_GetDriveState ent) {
            ent.CallBack(isDriver == false ? GPOData.GPOType.NULL : masterGPO.GetGPOType());
        }
        
        private void SetDriverGPO(bool isDriver) {
            if (this.isDriver == isDriver) {
                return;
            }
            this.isDriver = isDriver;
            bool isMainControl = !(isDriver && masterGPO.GetGPOType() == GPOData.GPOType.Role);
            masterGPO.Dispatcher(new SE_GPO.Event_PlayerDrive {
                IsDrive = isDriver,
                DriveGPO = isDriver == false ? null : iGPO,
            });
            mySystem.Dispatcher(new SE_AI.Event_DriveState() {
                IsDrive = isDriver,
                DriveGpoType = isDriver == false ? GPOData.GPOType.NULL : masterGPO.GetGPOType()
            });
            mySystem.Dispatcher(new SE_Network.Event_EnabledSyncNetworkTransform {
                IsTrue = isMainControl
            });
        }
    }
}