using System.Collections.Generic;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterManager : ManagerBase {
        private int perf_RoleCount = PerfAnalyzerKey.StringToHash("在线角色数量");
        private int perf_OffLineRoleCount = PerfAnalyzerKey.StringToHash("断线角色数量");
        private List<C_Character_Base> characterList = new List<C_Character_Base>();
        private float perfDeltaTime = 0.0f;

        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Register<CM_Character.AddCharacter>(OnAddCharacterCallBack);
            MsgRegister.Register<CM_Character.AddLocalCharacter>(OnAddLocalCharacterCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            characterList.Clear();
            MsgRegister.Unregister<CM_Character.AddCharacter>(OnAddCharacterCallBack);
            MsgRegister.Unregister<CM_Character.AddLocalCharacter>(OnAddLocalCharacterCallBack);
            RemoveAllRole();
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            SavePerfanalyzer();
        }

        private void SavePerfanalyzer() {
            if (NetworkData.IsStartServer) {
                return;
            }
            if (perfDeltaTime >= 0f) {
                perfDeltaTime -= Time.deltaTime;
                return;
            }
            perfDeltaTime = 0.3f;
            var roleCount = 0;
            var offLineRoleCount = 0;
            for (int i = 0; i < characterList.Count; i++) {
                var role = characterList[i];
                if (role.network != null) {
                    roleCount++;
                } else {
                    offLineRoleCount++;
                }
            }
            PerfAnalyzerAgent.SetCustomRecorder(perf_RoleCount, roleCount);
            PerfAnalyzerAgent.SetCustomRecorder(perf_OffLineRoleCount, offLineRoleCount);
        }

        private void OnAddLocalCharacterCallBack(CM_Character.AddLocalCharacter ent) {
            var roleInfo = (Proto_Login.TargetRpc_RoleInfo)ent.RoleInfoDoc;
            var netSync = (ICharacterSync)ent.iNetwork.GetNetworkSync();
            var character = (CharacterLocalSystem)GetCharacter(PlayerData.PlayerId);
            var mData = GpoSet.GetGPOMData(GpoSet.Id_Character);
            if (character == null) {
                AddSystem(delegate(CharacterLocalSystem system) {
                    system.SetPlayerId(PlayerData.PlayerId);
                    system.SetNickName(PlayerData.NickName);
                    system.SetCharacterData(roleInfo.gpoId, roleInfo.teamId, mData, ent.InDataDoc, true);
                    system.SetNetworkBehaviour((INetworkCharacter)ent.iNetwork, netSync);
                    character = system;
                    characterList.Add(system);
                });
            } else {
                character.SetCharacterData(roleInfo.gpoId, roleInfo.teamId, mData, ent.InDataDoc, true);
                character.SetNetworkBehaviour((INetworkCharacter)ent.iNetwork, netSync);
            }
            if (roleInfo.point != Vector3.zero) {
                character.SetLoginPoint(roleInfo.point, roleInfo.rota);
            }
            Debug.Log("客户端 - 创建本地角色:" + netSync.SyncNickName() + " GpoID:" + roleInfo.gpoId + " teamId:" +roleInfo.teamId);
        }

        private void OnAddCharacterCallBack(CM_Character.AddCharacter ent) {
            var netSync = (ICharacterSync)ent.iNetwork.GetNetworkSync();
            if (WarReportData.IsStartWarReport()) {
                AddOtherCharacter((INetworkCharacter)ent.iNetwork, ent.InDataDoc, netSync);
            } else {
                if (netSync.SyncPlayerId() != PlayerData.PlayerId && netSync.SyncTeamId() != 0) {
                    AddOtherCharacter((INetworkCharacter)ent.iNetwork, ent.InDataDoc, netSync);
                }
            }
            //Debug.Log("客户端 - 收到创建其他角色请求:" + ent.iNetwork);
        }


        private void AddOtherCharacter(INetworkCharacter network, IProto_Doc inData, ICharacterSync netSync) {
            //Debug.Log("客户端 - 创建其他角色:" + netSync.SyncNickName() + " GpoID:" + netSync.SyncGpoId());
            var character = GetCharacter(netSync.SyncPlayerId());
            var mData = GpoSet.GetGPOMData(GpoSet.Id_Character);
            if (character == null) {
                AddSystem(delegate(CharacterOtherSystem system) {
                    system.SetNickName(netSync.SyncNickName());
                    system.SetPlayerId(netSync.SyncPlayerId());
                    system.SetCharacterData(netSync.SyncGpoId(), netSync.SyncTeamId(), mData, inData, network.IsLocalPlayer());
                    system.SetNetworkBehaviour(network, netSync);
                    characterList.Add(system);
                });
            } else {
                character.SetCharacterData(netSync.SyncGpoId(), netSync.SyncTeamId(), mData, inData, network.IsLocalPlayer());
                character.SetNetworkBehaviour(network, netSync);
            }
        }

        private C_Character_Base GetCharacter(long playerId) {
            for (int i = 0; i < characterList.Count; i++) {
                var character = characterList[i];
                if (character.PlayerId == playerId) {
                    return character;
                }
            }
            return null;
        }

        private void RemoveAllRole() {
            for (int i = 0; i < characterList.Count; i++) {
                var role = characterList[i];
                role.Clear();
            }
            characterList.Clear();
        }
    }
}