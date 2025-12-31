using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterDrive : ServerGPOOnDrive {
        private INetworkCharacter characterNetwork;

        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Drive.Cmd_DriveAnim.ID, OnCmdDriveAnimCallBack);
            RemoveProtoCallBack(Proto_Drive.Cmd_SyncTankUpperBodyRota.ID, OnCmdSyncTankUpperBodyRotaCallBack);
            RemoveProtoCallBack(Proto_Drive.Cmd_DeviceFire.ID, OnCmdDeviceFireCallBack);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            characterNetwork = (INetworkCharacter)networkBase;
            AddProtoCallBack(Proto_Drive.Cmd_DriveAnim.ID, OnCmdDriveAnimCallBack);
            AddProtoCallBack(Proto_Drive.Cmd_SyncTankUpperBodyRota.ID, OnCmdSyncTankUpperBodyRotaCallBack);
            AddProtoCallBack(Proto_Drive.Cmd_DeviceFire.ID, OnCmdDeviceFireCallBack);
        }

        protected override void UpdateDrivePoint() {
            if (characterNetwork.GetPoint() == Vector3.zero) {
                return;
            }
            // 角色操控的位置需要以角色的位置为准
            driveGPO.Dispatcher(new SE_AI.Event_DriverPointRota {
                Point = characterNetwork.GetPoint(),
                Rota = characterNetwork.GetRota()
            });
            if (driveTransform != null) {
                iEntity.SetPoint(driveTransform.position);
                iEntity.SetRota(driveTransform.rotation);
            }
        }
        private void OnCmdDriveAnimCallBack(INetwork network, IProto_Doc docData) {
            var cmdData = (Proto_Drive.Cmd_DriveAnim)docData;
            if (driveGPO == null) {
                return;
            }
            Rpc(new Proto_Drive.Rpc_DriveAnim {
                animId = cmdData.animId
            });
        }
        private void OnCmdSyncTankUpperBodyRotaCallBack(INetwork network, IProto_Doc docData) {
            var cmdData = (Proto_Drive.Cmd_SyncTankUpperBodyRota)docData;
            if (driveGPO == null) {
                return;
            }
            driveGPO.Dispatcher(new SE_AI.Event_PlayerDriveSetTankUpperBodyRota {
                Rota = cmdData.eulerAngles
            });
        }

        private void OnCmdDeviceFireCallBack(INetwork network, IProto_Doc docData) {
            var cmdData = (Proto_Drive.Cmd_DeviceFire)docData;
            if (driveGPO == null) {
                return;
            }
            driveGPO.Dispatcher(new SE_AI.Event_PlayerDriveFireForPoints {
                Points = cmdData.points,
                CallBack = isTrue => {
                    TargetRpc(network, new Proto_Drive.TargetRpc_DeviceFireState {
                        isSuccess = isTrue
                    });
                }
            });
        }
        
    }
}