using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIDrive : ComponentBase {
        private ClientGPO driveGPO;
        private int driveGPOID = 0;
        private EntityBase entityBase;
        private Transform driveTransform = null;

        private bool isDrive {
            get { return driveGPOID > 0; }
        }

        protected override void OnAwake() {
            mySystem.Register<CE_AI.Event_GetDriveing>(OnGetDriveingCallBack);
            mySystem.Register<CE_GPO.Event_DriveGPO>(OnDriverCallBack);
            mySystem.Register<CE_AI.Event_DriverPointRota>(OnDriverPointRotaCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<CE_AI.Event_DriverPointRota>(OnDriverPointRotaCallBack);
            mySystem.Unregister<CE_AI.Event_GetDriveing>(OnGetDriveingCallBack);
            mySystem.Unregister<CE_GPO.Event_DriveGPO>(OnDriverCallBack);
            ClearDriveGPO();
            base.OnClear();
        }
        
        private void OnUpdate(float deltaTime) {
            if (iEntity.GetPoint() == Vector3.zero || isSetEntityObj == false || driveGPOID == 0) {
                return;
            }
            driveGPO.Dispatcher(new CE_GPO.Event_DriverPointRota {
                Point = driveTransform.position,
                Rota = driveTransform.rotation
            });
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            driveTransform = iEntity.GetBodyTran(GPOData.PartEnum.Driver);
        }

        private void OnDriverPointRotaCallBack(ISystemMsg body, CE_AI.Event_DriverPointRota ent) {
            if (driveGPO == null) {
                return;
            }
            mySystem.Dispatcher(new CE_GPO.Event_SetTransformPointRota {
                Point = ent.Point,
                Rota = ent.Rota
            });
        }

        private void OnDriverCallBack(ISystemMsg body, CE_GPO.Event_DriveGPO ent) {
            driveGPO = (ClientGPO)ent.PlayerDriveGPO;
            var enabledDriveMove = false;
            if (driveGPO == null) {
                driveGPOID = 0;
                var rota = Vector3.zero;
                rota.y = iEntity.GetEulerAngles().y;
                iEntity.SetEulerAngles(rota);
            } else {
                driveGPOID = driveGPO.GetGpoID();
                enabledDriveMove = driveGPO.IsLocalGPO();
            }
            mySystem.Dispatcher(new CE_AI.Event_EnabledDriveMove {
                IsTrue = enabledDriveMove
            });
        }

        private void ClearDriveGPO() {
            if (this.driveGPO == null) {
                return;
            }
            this.driveGPO = null;
            driveGPOID = 0;
        }

        private void OnGetDriveingCallBack(ISystemMsg body, CE_AI.Event_GetDriveing ent) {
            ent.CallBack.Invoke(isDrive);
        }
    }
}