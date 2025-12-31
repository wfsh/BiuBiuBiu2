using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;

using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashCharacterNetwork : ServerCharacterComponent {
        private S_Character_Base system;
        public CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        public CharacterData.FlyType flyType = CharacterData.FlyType.None;
        public string useHoldOn = "";
        public bool isDodge = false;
        private bool isLockSelf = true;
        private IGPO driveGPO;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_PlayerDrive>(OnOnDriveGPOCallBack);
            mySystem.Register<SE_Character.SetSausageLockGPO>(OnLookGPOIDCallBack);
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
            RemoveProtoCallBack(Proto_Character.Cmd_Dodga.ID, OnDodgaCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_CameraVRota.ID, CameraVRotaCallBack);
            RemoveProtoCallBack(Proto_Weapon.Cmd_HoldOn.ID, SetHoldOnSign);
            RemoveProtoCallBack(Proto_Weapon.Cmd_Throw.ID, OnThrowCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_FallToGrounded.ID, OnFallToGroundedCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_Ping.ID, OnPingCallBack);
            mySystem.Unregister<SE_GPO.Event_PlayerDrive>(OnOnDriveGPOCallBack);
            mySystem.Unregister<SE_Character.SetSausageLockGPO>(OnLookGPOIDCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void OnLookGPOIDCallBack(ISystemMsg body, SE_Character.SetSausageLockGPO ent) {
            if (ent.LockGpo == null || ent.LockGpo == iGPO) {
                isLockSelf = true;
            } else {
                isLockSelf = false;
            }
        }
        
        protected override void OnSetNetwork() {
            AddCmdCallBack();
        }

        private void AddCmdCallBack() {
            AddProtoCallBack(Proto_Character.Cmd_JumpType.ID, OnJumpTypeCallBack);
            AddProtoCallBack(Proto_Character.Cmd_FlyType.ID, OnFlyTypeCallBack);
            AddProtoCallBack(Proto_Character.Cmd_Dodga.ID, OnDodgaCallBack);
            AddProtoCallBack(Proto_Character.Cmd_CameraVRota.ID, CameraVRotaCallBack);
            AddProtoCallBack(Proto_Weapon.Cmd_HoldOn.ID, SetHoldOnSign);
            AddProtoCallBack(Proto_Weapon.Cmd_Throw.ID, OnThrowCallBack);
            AddProtoCallBack(Proto_Character.Cmd_FallToGrounded.ID, OnFallToGroundedCallBack);
            AddProtoCallBack(Proto_Character.Cmd_Ping.ID, OnPingCallBack);
        }

        private void OnUpdate(float deltaTime) {
            if (networkBase == null || networkBase.IsDestroy()) {
                return;
            }
            if (driveGPO == null && isLockSelf == true) {
                var networkPoint = characterNetwork.GetPoint();
                if (networkPoint != Vector3.zero) {
                    iEntity.SetPoint(characterNetwork.GetPoint());
                }
                iEntity.SetRota(characterNetwork.GetRota());
            }
            SetSync();
        }

        public void OnOnDriveGPOCallBack(ISystemMsg body, SE_GPO.Event_PlayerDrive entData) {
            driveGPO = entData.DriveGPO;
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
            }
            if (characterSync.FlyType() != flyType) {
                characterSync.FlyType(flyType);
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