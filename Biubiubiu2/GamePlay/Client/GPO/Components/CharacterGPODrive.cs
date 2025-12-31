using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterGPODrive : ComponentBase {
        private bool isDrive {
            get { return driveGPOID != 0; }
        }

        private int driveGPOID = 0;
        protected IGPO driveGPO = null;

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<CM_GPO.AddGPO>(OnAddGPOCallBack);
            mySystem.Register<CE_GPO.Event_DriverPointRota>(OnDriverPointRotaCallBack);
            mySystem.Register<CE_GPO.Event_MoveDir>(OnMoveDirCallBack);
        }

        protected override void OnClear() {
            ClearDirveGPO();
            RemoveProtoCallBack(Proto_Drive.Rpc_DriveGPOID.ID, OnRpcDriveGPOIDCallBack);
            RemoveProtoCallBack(Proto_Drive.TargetRpc_DriveGPOID.ID, OnTargetRpcDriveGPOIDCallBack);
            RemoveProtoCallBack(Proto_Drive.Rpc_DriveAnim.ID, OnRpcDriveAnimCallBack);
            mySystem.Unregister<CE_GPO.Event_DriverPointRota>(OnDriverPointRotaCallBack);
            mySystem.Unregister<CE_GPO.Event_MoveDir>(OnMoveDirCallBack);
            MsgRegister.Unregister<CM_GPO.AddGPO>(OnAddGPOCallBack);
            base.OnClear();
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Drive.Rpc_DriveGPOID.ID, OnRpcDriveGPOIDCallBack);
            AddProtoCallBack(Proto_Drive.TargetRpc_DriveGPOID.ID, OnTargetRpcDriveGPOIDCallBack);
            AddProtoCallBack(Proto_Drive.Rpc_DriveAnim.ID, OnRpcDriveAnimCallBack);
        }
        
        private void OnDriverPointRotaCallBack(ISystemMsg body, CE_GPO.Event_DriverPointRota ent) {
            if (isDrive == false) {
                return;
            }
            iEntity.SetPoint(ent.Point);
            iEntity.SetRota(ent.Rota);
        }
        
        private void OnMoveDirCallBack(ISystemMsg body, CE_GPO.Event_MoveDir ent) {
            if (isDrive == false) {
                return;
            }
            driveGPO?.Dispatcher(new CE_AI.Event_MoveDir {
                MoveX = ent.MoveH, MoveZ = ent.MoveV
            });
        }
        
        private void OnRpcDriveAnimCallBack(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_Drive.Rpc_DriveAnim)docData;
            if (driveGPO == null) {
                return;
            }
            var animConfig = AIAnimConfig.Get(AIAnimConfig.HTR);
            var playAnim = animConfig.GetPlaySign(rpcData.animId);
            if (playAnim == "") {
                return;
            }
            mySystem.Dispatcher(new CE_Character.PlayAnimSign {
                PlaySign = playAnim,
            });
            driveGPO.Dispatcher(new CE_AI.Event_SetPlayAnimSign {
                AnimSign = playAnim
            });
        }
        
        private void OnRpcDriveGPOIDCallBack(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_Drive.Rpc_DriveGPOID)docData;
            SetDriveGPOID(rpcData.gpoId);
        }

        private void OnTargetRpcDriveGPOIDCallBack(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_Drive.TargetRpc_DriveGPOID)docData;
            SetDriveGPOID(rpcData.gpoId);
        }

        private void SetDriveGPOID(int gpoId) {
            driveGPOID = gpoId;
            if (driveGPOID == 0) {
                mySystem.Dispatcher(new CE_GPO.Event_PlayerDriveGPO {
                    DriveGPO = null
                });
                driveGPO?.Dispatcher(new CE_GPO.Event_DriveGPO {
                    PlayerDriveGPO = null
                });
                ClearDirveGPO();
            } else {
                MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                    GpoId = driveGPOID, CallBack = GetGPOCallBack
                });
            }
        }

        private void OnAddGPOCallBack(CM_GPO.AddGPO ent) {
            if (driveGPOID == 0) {
                return;
            }
            GetGPOCallBack(ent.IGpo);
        }

        private void GetGPOCallBack(IGPO gpo) {
            if (gpo == null || gpo.GetGpoID() != driveGPOID) {
                return;
            }
            driveGPO = gpo;
            mySystem.Dispatcher(new CE_GPO.Event_PlayerDriveGPO {
                DriveGPO = driveGPO
            });
            driveGPO.Dispatcher(new CE_GPO.Event_DriveGPO {
                PlayerDriveGPO = iGPO
            });
        }
        
        private void ClearDirveGPO() {
            if (driveGPO == null) {
                return;
            }
            driveGPOID = 0;
            driveGPO = null;
        }
    }
}