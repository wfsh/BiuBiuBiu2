using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;

using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterNetwork : ServerCharacterComponent {
        public struct InitData : SystemBase.IComponentInitData {
            public Action<ServerCharacterNetwork> CallBack;
        }
        private S_Character_Base system;
        public CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        public CharacterData.FlyType flyType = CharacterData.FlyType.None;
        public CharacterData.StandType standType = CharacterData.StandType.Stand;
        public string useHoldOn = "";
        public bool isDodge = false;
        private bool isOnline = true;
        private float deltaCheckOnline = 0;
        private IGPO driveGPO;
        private Vector3 prevFramePoint = Vector3.zero;
        private Quaternion prevFrameRota = Quaternion.identity;
        private Action<ServerCharacterNetwork> sendProtoAction;
        private ITargetRpc sendProto;
        
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_PlayerDrive>(OnOnDriveGPOCallBack);
            mySystem.Register<SE_Character.Event_SetHoldOnSign>(OnSetHoldOnSign);
            mySystem.Register<SE_Character.StandTypeChange>(OnEventStandTypeChange);
            var initData = (InitData)initDataBase;
            sendProtoAction = initData.CallBack;
        }

        protected override void OnStart() {
            base.OnStart();
            system = (S_Character_Base)mySystem;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Character.Cmd_JumpType.ID, OnJumpTypeCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_FlyType.ID, OnFlyTypeCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_StandType.ID, OnStandTypeCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_Dodga.ID, OnDodgaCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_CameraVRota.ID, CameraVRotaCallBack);
            RemoveProtoCallBack(Proto_Weapon.Cmd_HoldOn.ID, SetHoldOnSign);
            RemoveProtoCallBack(Proto_Weapon.Cmd_Throw.ID, OnThrowCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_FallToGrounded.ID, OnFallToGroundedCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_Ping.ID, OnPingCallBack);
            mySystem.Unregister<SE_GPO.Event_PlayerDrive>(OnOnDriveGPOCallBack);
            mySystem.Unregister<SE_Character.Event_SetHoldOnSign>(OnSetHoldOnSign);
            mySystem.Unregister<SE_Character.StandTypeChange>(OnEventStandTypeChange);
            RemoveUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            AddCmdCallBack();
        }
        
        public void SetSpawnRPC(ITargetRpc protoData) {
            sendProto = protoData;
        }
        
        protected override ITargetRpc SyncData() {
            sendProtoAction.Invoke(this);
            return sendProto;
        }
        
        private void AddCmdCallBack() {
            AddProtoCallBack(Proto_Character.Cmd_JumpType.ID, OnJumpTypeCallBack);
            AddProtoCallBack(Proto_Character.Cmd_FlyType.ID, OnFlyTypeCallBack);
            AddProtoCallBack(Proto_Character.Cmd_StandType.ID, OnStandTypeCallBack);
            AddProtoCallBack(Proto_Character.Cmd_Dodga.ID, OnDodgaCallBack);
            AddProtoCallBack(Proto_Character.Cmd_CameraVRota.ID, CameraVRotaCallBack);
            AddProtoCallBack(Proto_Weapon.Cmd_HoldOn.ID, SetHoldOnSign);
            AddProtoCallBack(Proto_Weapon.Cmd_Throw.ID, OnThrowCallBack);
            AddProtoCallBack(Proto_Character.Cmd_FallToGrounded.ID, OnFallToGroundedCallBack);
            AddProtoCallBack(Proto_Character.Cmd_Ping.ID, OnPingCallBack);
        }

        private void OnUpdate(float deltaTime) {
            UpdatreCheckIsOnline();
            if (networkBase == null || networkBase.IsDestroy()) {
                return;
            }
            SendPointToReport();
            SendRotaToReport();
            if (driveGPO == null) {
                var networkPoint = characterNetwork.GetPoint();
                if (networkPoint != Vector3.zero) {
                    var prevPoint = iEntity.GetPoint();
                    var prevPos = new Vector2(prevPoint.x, prevPoint.z);
                    var newPos = new Vector2(networkPoint.x, networkPoint.z);
                    iEntity.SetPoint(networkPoint);
                    if (!prevPos.Equals(newPos)) {
                        Dispatcher(new SE_GPO.Event_MoveDistance {
                            GPOId = GpoID,
                            moveDistance = Vector2.Distance(prevPos, newPos)
                        });
                    }
                }
                iEntity.SetRota(characterNetwork.GetRota());
            }
            SetSync();
            prevFramePoint = characterNetwork.GetPoint();
            prevFrameRota = characterNetwork.GetRota();
        }

        private void SendPointToReport() {
            if (Vector3.Distance(prevFramePoint, characterNetwork.GetPoint()) < 0.01f) {
                return;
            }
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterPoint {
                NetworkId = networkBase.GetNetworkId(),
                Point = characterNetwork.GetPoint(),
            });
        }
        
        private void SendRotaToReport() {
            if (Quaternion.Angle(prevFrameRota, characterNetwork.GetRota()) < 1f) {
                return;
            }
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterRota {
                NetworkId = networkBase.GetNetworkId(),
                Rota = characterNetwork.GetRota(),
            });
        }
        
        // 根据时间间隔判断 network 是否 = null 判断 isOnline 是否 =  true
        private void UpdatreCheckIsOnline() {
            if (deltaCheckOnline < 0.5f) {
                deltaCheckOnline += Time.deltaTime;
                return;
            }
            deltaCheckOnline = 0;
            var isOnline = true;
            if (networkBase == null || networkBase.IsOnline() == false) {
                isOnline = false;
            }
            if (isOnline == this.isOnline) {
                return;
            }
            this.isOnline = isOnline;
            this.mySystem.Dispatcher(new SE_Network.Event_SetIsOnline {
                GpoId = iGPO.GetGpoID(),
                IsOnline = isOnline
            });
        }

        public void OnOnDriveGPOCallBack(ISystemMsg body, SE_GPO.Event_PlayerDrive entData) {
            driveGPO = entData.DriveGPO;
        }

        private void OnSetHoldOnSign(ISystemMsg body, SE_Character.Event_SetHoldOnSign entData) {
            useHoldOn = entData.HoldOnSign;
        }
        
        private void SetSync() {
            if (characterSync.SyncPlayerId() != system.PlayerId) {
                characterSync.SyncPlayerId(system.PlayerId);
            }
            if (characterSync.SyncNickName() != system.NickName) {
                characterSync.SyncNickName(system.NickName);
            }
            if (characterSync.JumpType() != jumpType) {
                characterSync.JumpType(jumpType);
                if (jumpType is CharacterData.JumpType.Jump or CharacterData.JumpType.AirJump) {
                    mySystem.Dispatcher(new SE_GPO.Event_Jump {
                        GPOId = GpoID,
                        JumpType = jumpType
                    });
                }
            }
            if (characterSync.FlyType() != flyType) {
                characterSync.FlyType(flyType);
            }
            if (characterSync.StandType() != standType) {
                characterSync.StandType(standType);
            }
            if (characterSync.IsDodge() != isDodge) {
                characterSync.IsDodge(isDodge);
            }
            if (characterSync.UseHoldOn() != useHoldOn) {
                characterSync.UseHoldOn(useHoldOn);
            }
            if (characterSync.SyncGpoId() != system.GpoId) {
                characterSync.SyncGpoId(system.GpoId);
            }
            if (characterSync.SyncTeamId() != system.TeamId) {
                characterSync.SyncTeamId(system.TeamId);
            }
        }
        
        private void OnJumpTypeCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Cmd_JumpType)cmdData;
            jumpType = data.jumpType;
        }

        private void OnFlyTypeCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Cmd_FlyType)cmdData;
            flyType = data.flyType;
        }

        private void OnStandTypeCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Cmd_StandType)cmdData;
            standType = data.standType;
        }
        
        private void OnEventStandTypeChange(ISystemMsg body, SE_Character.StandTypeChange entData) {
            standType = entData.StandType;
        }

        private void OnDodgaCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Cmd_Dodga)cmdData;
            isDodge = data.isDodge;
        }

        private void CameraVRotaCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Cmd_CameraVRota)cmdData;
            Rpc(new Proto_Character.Rpc_CameraVRota {
                vRota = data.vRota,
            });
        }

        public void SetHoldOnSign(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Weapon.Cmd_HoldOn)cmdData;
            useHoldOn = data.holdSign;
        }

        public void OnThrowCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            Rpc(new Proto_Weapon.Rpc_Throw());
        }
        
        public void OnFallToGroundedCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            Rpc(new Proto_Character.Rpc_FallToGrounded());
        }

        public void OnPingCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Cmd_Ping)cmdData;
            characterNetwork.TargetRpc(characterNetwork,new Proto_Character.TargetRpc_Ping {
                index = data.index
            });
        }
    }
}