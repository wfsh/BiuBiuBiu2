using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOOnDrive : ServerNetworkComponentBase {
        protected IGPO driveGPO;
        protected Transform driveTransform = null;

        protected override void OnAwake() {
            mySystem.Register<SE_GPO.Event_PlayerDrive>(OnOnDriveGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            ClearDriveGPO();
            mySystem.Unregister<SE_GPO.Event_PlayerDrive>(OnOnDriveGPOCallBack);
            RemoveUpdate(OnUpdate);
        }

        protected override ITargetRpc SyncData() {
            if (driveGPO == null) {
                return new Proto_Drive.TargetRpc_DriveGPOID {
                    gpoId = 0, drivePos = Vector3.zero, driveRota = Quaternion.identity,
                };
            } else {
                return new Proto_Drive.TargetRpc_DriveGPOID {
                    gpoId = driveGPO.GetGpoID(), drivePos = driveGPO.GetPoint(), driveRota = driveGPO.GetRota()
                };
            }
        }

        private void OnUpdate(float deltaTime) {
            if (networkBase == null || networkBase.IsDestroy()) {
                return;
            }
            if (driveGPO != null) {
                UpdateDrivePoint();
            }
        }

        virtual protected void UpdateDrivePoint() {
            if (driveTransform != null) {
                iEntity.SetPoint(driveTransform.position);
                iEntity.SetRota(driveTransform.rotation);
            }
        }

        private void OnOnDriveGPOCallBack(ISystemMsg body, SE_GPO.Event_PlayerDrive ent) {
            ClearDriveGPO();
            if (ent.IsDrive) {
                driveGPO = ent.DriveGPO;
                GetDriveTransform();
            }
            Rpc(new Proto_Drive.Rpc_DriveGPOID {
                gpoId = driveGPO != null ? driveGPO.GetGpoID() : 0
            });
        }

        private void ClearDriveGPO() {
            driveGPO = null;
            driveTransform = null;
        }

        private void GetDriveTransform() {
            if (driveGPO == null) {
                return;
            }
            driveTransform = driveGPO.GetBodyTran(GPOData.PartEnum.Driver);
            if (driveTransform == null) {
                Debug.LogError($"{driveGPO.GetName()} 没有驾驶员座位");
            }
        }
    }
}